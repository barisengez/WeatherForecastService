using Business.Dtos;

namespace Business.Services;

public interface IWeatherForecastApplicationService
{
    public Task<WeatherForecastDto> AddWeatherForecastAsync(AddWeatherForecastDto dto);
    public Task<List<WeatherForecastDto>> GetWeeklyWeatherForecastAsync();
}