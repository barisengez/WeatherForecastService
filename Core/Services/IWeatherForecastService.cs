using Core.Entities;

namespace Core.Services;

public interface IWeatherForecastService
{
    public Task AddWeatherForecastAsync(WeatherForecast forecast);
    public Task<List<WeatherForecast>> GetWeatherForecastAsync(DateOnly fromDate, int maxCount);
}