using Core.Entities;
using Core.Repositories;

namespace Core.Services
{
    public class WeatherForecastService(IWeatherForecastRepository repository) : IWeatherForecastService
    {
        public async Task AddWeatherForecastAsync(WeatherForecast forecast)
        {
            await repository.SaveOrUpdateAsync(forecast);
        }

        public async Task<List<WeatherForecast>> GetWeatherForecastAsync(DateOnly fromDate, int maxCount)
        {
            return await repository.GetWeatherForecastsAsync(fromDate, maxCount);
        }
    }
}
