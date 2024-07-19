using Business.Dtos;

namespace Business.Services
{
    public interface IWeatherForecastApplicationService
    {
        public Task AddWeatherForecastAsync(AddWeatherForecastDto dto);
        public Task<List<WeatherForecastDto>> GetWeeklyWeatherForecastAsync();
    }
}
