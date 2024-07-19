using Core.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL.Repositories;

public class WeatherForecastRepository(WeatherForecastDbContext context) : IWeatherForecastRepository
{
    private const int MaxWeatherForecastCountToReturn = 1000;

    public async Task SaveOrUpdateAsync(WeatherForecast forecast)
    {
        var existingRecord = await context.WeatherForecasts.Where(p => p.Date == forecast.Date).FirstOrDefaultAsync();
        if (existingRecord != null)
        {
            existingRecord.Temperature = forecast.Temperature;
            context.WeatherForecasts.Update(existingRecord);
        }
        else
        {
            await context.WeatherForecasts.AddAsync(forecast);
        }

        await context.SaveChangesAsync();
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