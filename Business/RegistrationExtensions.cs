using Business.Mappings;
using Business.Services;
using Business.Validators;
using Core.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Business;

public static class RegistrationExtensions
{
    public static IServiceCollection AddWeatherForecastBusiness(this IServiceCollection services)
    {
        services
            .AddAutoMapper(typeof(WeatherForecastMappingProfile), typeof(AddWeatherForecastMappingProfile))
            .AddScoped<IWeatherForecastApplicationService, WeatherForecastApplicationService>()
            .AddScoped<IWeatherForecastService, WeatherForecastService>()
            .AddValidatorsFromAssemblyContaining<AddWeatherForecastDtoValidator>();

        return services;
    }
}