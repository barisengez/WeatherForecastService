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
        var returnedForecast = await repository.AddOrUpdateAsync(forecastToSave);

        // Assert
        var addedForecast = await contextFixture.Context.WeatherForecasts.FirstOrDefaultAsync(f => f.Date == forecastToSave.Date);
        Assert.NotNull(addedForecast);
        Assert.Equal(forecastToSave.Temperature, addedForecast.Temperature);
        Assert.Equal(forecastToSave.Date, addedForecast.Date);
        Assert.Equal(forecastToSave.Temperature, returnedForecast.Temperature);
        Assert.Equal(forecastToSave.Date, returnedForecast.Date);
    }

    [Fact]
    public async Task SaveOrUpdateAsync_Updates_Existing_Forecast_When_Record_Exists()
    {
        // Arrange
        contextFixture.ClearDatabase();
        var loggerMock = new Mock<ILogger<WeatherForecastRepository>>();
        var repository = new WeatherForecastRepository(contextFixture.Context, loggerMock.Object);
        var existingForecast = _fixture.Create<WeatherForecast>();
        var forecastToUpdate = _fixture.Create<WeatherForecast>();
        await contextFixture.Context.WeatherForecasts.AddAsync(existingForecast);
        await contextFixture.Context.SaveChangesAsync();

        // Act
        var returnedForecast = await repository.AddOrUpdateAsync(forecastToUpdate);

        // Assert
        var updatedRecord = await contextFixture.Context.WeatherForecasts
            .FirstOrDefaultAsync(f => f.Date == forecastToUpdate.Date);
        Assert.NotNull(updatedRecord);
        Assert.Equal(forecastToUpdate.Temperature, updatedRecord.Temperature);
        Assert.Equal(forecastToUpdate.Date, updatedRecord.Date);
        Assert.Equal(forecastToUpdate.Temperature, returnedForecast.Temperature);
        Assert.Equal(forecastToUpdate.Date, returnedForecast.Date);
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