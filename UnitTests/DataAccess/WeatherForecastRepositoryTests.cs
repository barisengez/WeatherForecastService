using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using DataAccess.SQL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.Helpers;

namespace UnitTests.DataAccess;

public class WeatherForecastRepositoryTests(WeatherForecastDbContextFixture contextFixture)
    : IClassFixture<WeatherForecastDbContextFixture>
{
    private readonly IFixture _fixture = new Fixture()
        .Customize(new CompositeCustomization(new AutoMoqCustomization(), new DateOnlyFixtureCustomization()));

    [Fact]
    public async Task SaveOrUpdateAsync_Saves_New_Forecast_When_No_Existing_Record()
    {
        // Arrange
        contextFixture.ClearDatabase();
        var loggerMock = new Mock<ILogger<WeatherForecastRepository>>();
        var repository = new WeatherForecastRepository(contextFixture.Context, loggerMock.Object);
        var forecastToSave = _fixture.Create<WeatherForecast>();

        // Act
        await repository.SaveOrUpdateAsync(forecastToSave);

        // Assert
        var savedForecast = await contextFixture.Context.WeatherForecasts.FirstOrDefaultAsync(f => f.Date == forecastToSave.Date);
        Assert.NotNull(savedForecast);
        Assert.Equal(forecastToSave.Temperature, savedForecast.Temperature);
        Assert.Equal(forecastToSave.Date, savedForecast.Date);
    }

    [Fact]
    public async Task SaveOrUpdateAsync_Updates_Existing_Forecast_When_Record_Exists()
    {
        // Arrange
        contextFixture.ClearDatabase();
        var loggerMock = new Mock<ILogger<WeatherForecastRepository>>();
        var repository = new WeatherForecastRepository(contextFixture.Context, loggerMock.Object);
        var existingForecast = _fixture.Create<WeatherForecast>();
        var updatedForecast = _fixture.Create<WeatherForecast>();
        await contextFixture.Context.WeatherForecasts.AddAsync(existingForecast);
        await contextFixture.Context.SaveChangesAsync();

        // Act
        await repository.SaveOrUpdateAsync(updatedForecast);

        // Assert
        var updatedRecord = await contextFixture.Context.WeatherForecasts
            .FirstOrDefaultAsync(f => f.Date == updatedForecast.Date);
        Assert.NotNull(updatedRecord);
        Assert.Equal(updatedForecast.Temperature, updatedRecord.Temperature);
        Assert.Equal(updatedForecast.Date, updatedRecord.Date);
    }

    [Fact]
    public async Task GetWeatherForecastsAsync_Returns_WeatherForecasts_From_Specified_Date()
    {
        // Arrange
        contextFixture.ClearDatabase();
        var loggerMock = new Mock<ILogger<WeatherForecastRepository>>();
        var repository = new WeatherForecastRepository(contextFixture.Context, loggerMock.Object);

        await contextFixture.Context.WeatherForecasts.AddRangeAsync(
            new WeatherForecast { Date = new DateOnly(2024, 1, 1), Temperature = 10 },
            new WeatherForecast { Date = new DateOnly(2024, 1, 2), Temperature = 12 },
            new WeatherForecast { Date = new DateOnly(2024, 1, 3), Temperature = 14 }
        );
        await contextFixture.Context.SaveChangesAsync();
        var fromDate = new DateOnly(2024, 1, 2);

        // Act
        var result = await repository.GetWeatherForecastsAsync(fromDate, 10);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(new DateOnly(2024, 1, 2), result[0].Date);
        Assert.Equal(new DateOnly(2024, 1, 3), result[1].Date);
    }

    [Fact]
    public async Task GetWeatherForecastsAsync_Limits_Result_AccordingTo_MaxCount()
    {
        // Arrange
        contextFixture.ClearDatabase();
        var loggerMock = new Mock<ILogger<WeatherForecastRepository>>();
        var repository = new WeatherForecastRepository(contextFixture.Context, loggerMock.Object);

        await contextFixture.Context.WeatherForecasts.AddRangeAsync(
            new WeatherForecast { Date = new DateOnly(2024, 1, 1), Temperature = 10 },
            new WeatherForecast { Date = new DateOnly(2024, 1, 2), Temperature = 12 },
            new WeatherForecast { Date = new DateOnly(2024, 1, 3), Temperature = 14 }
        );
        await contextFixture.Context.SaveChangesAsync();
        var fromDate = new DateOnly(2024, 1, 2);

        // Act
        var result = await repository.GetWeatherForecastsAsync(fromDate, 1);

        // Assert
        Assert.Single(result);
        Assert.Equal(new DateOnly(2024, 1, 2), result[0].Date);
    }
}