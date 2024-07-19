﻿using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Moq;
using UnitTests.Helpers;

namespace UnitTests.Core;

public class WeatherForecastServiceTests
{
    private readonly IFixture _fixture = new Fixture()
        .Customize(new CompositeCustomization(new AutoMoqCustomization(), new DateOnlyFixtureCustomization()));

    [Fact]
    public async Task GetWeatherForecastAsync_Calls_Repository_With_Correct_Parameters()
    {
        // Arrange
        var mockRepository = new Mock<IWeatherForecastRepository>();
        var fromDate = _fixture.Create<DateOnly>();
        var maxCount = _fixture.Create<int>();
        var expectedForecasts = _fixture.CreateMany<WeatherForecast>(3).ToList();
        mockRepository.Setup(repo => repo.GetWeatherForecastsAsync(fromDate, maxCount))
            .ReturnsAsync(expectedForecasts).Verifiable();
        var service = new WeatherForecastService(mockRepository.Object);

        // Act
        var result = await service.GetWeatherForecastAsync(fromDate, maxCount);

        // Assert
        Assert.Equal(expectedForecasts, result);
        mockRepository.Verify();
    }

    [Fact]
    public async Task AddWeatherForecastAsync_Calls_Repository_With_Correct_Parameters()
    {
        // Arrange
        var mockRepository = new Mock<IWeatherForecastRepository>();
        var forecastToSave = _fixture.Create<WeatherForecast>();
        mockRepository.Setup(repo => repo.SaveOrUpdateAsync(forecastToSave)).Returns(Task.CompletedTask).Verifiable();
        var service = new WeatherForecastService(mockRepository.Object);

        // Act
        await service.AddWeatherForecastAsync(forecastToSave);

        // Assert
        mockRepository.Verify();
    }
}