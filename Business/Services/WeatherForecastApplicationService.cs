﻿using AutoMapper;
using Business.Dtos;
using Core.Entities;
using Core.Services;

namespace Business.Services;

public class WeatherForecastApplicationService(IWeatherForecastService weatherForecastService, IMapper mapper)
    : IWeatherForecastApplicationService
{
    private const int WeeklyDayCount = 7;

    public async Task<WeatherForecastDto> AddWeatherForecastAsync(AddWeatherForecastDto dto)
    {
        var forecast = mapper.Map<WeatherForecast>(dto);
        var addedForecast = await weatherForecastService.AddWeatherForecastAsync(forecast);

        return mapper.Map<WeatherForecastDto>(addedForecast);
    }

    public async Task<List<WeatherForecastDto>> GetWeeklyWeatherForecastAsync()
    {
        var forecasts = await weatherForecastService.GetWeatherForecastAsync(DateOnly.FromDateTime(DateTime.Now), WeeklyDayCount);
        return mapper.Map<List<WeatherForecastDto>>(forecasts);
    }
}