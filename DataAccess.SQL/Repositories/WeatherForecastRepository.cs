using Core.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.SQL.Repositories;

public class WeatherForecastRepository(WeatherForecastDbContext context, ILogger<WeatherForecastRepository> logger) : IWeatherForecastRepository
{
    private const int MaxWeatherForecastCountToReturn = 1000;

    public async Task<WeatherForecast> AddOrUpdateAsync(WeatherForecast forecast)
    {
        var existingRecord = await context.WeatherForecasts.Where(p => p.Date == forecast.Date).FirstOrDefaultAsync();

        if (existingRecord != null)
        {
            existingRecord.Temperature = forecast.Temperature;
            logger.LogInformation($"Forecast for date {forecast.Date} already exists and being updated");
        }
        else
        {
            context.WeatherForecasts.Add(forecast);
            logger.LogInformation($"Forecast for date {forecast.Date} does not exist and is being added");
        }

        await context.SaveChangesAsync();

        return forecast;
    }

    public async Task<List<WeatherForecast>> GetWeatherForecastsAsync(DateOnly fromDate, int maxCount)
    {
        return await context.WeatherForecasts
            .Where(p => p.Date >= fromDate)
            .OrderBy(p => p.Date)
            .Take(Math.Min(maxCount, MaxWeatherForecastCountToReturn))
            .ToListAsync();
    }
}