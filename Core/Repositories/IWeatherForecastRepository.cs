using Core.Entities;

namespace Core.Repositories;

public interface IWeatherForecastRepository
{
    Task<List<WeatherForecast>> GetWeatherForecastsAsync(DateOnly fromDate, int maxCount);
    Task<WeatherForecast> AddOrUpdateAsync(WeatherForecast forecast);
}