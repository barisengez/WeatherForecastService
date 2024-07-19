using Business.Mappings;
using Business.Services;
using Core.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddWeatherForecastBusiness(this IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(WeatherForecastMappingProfile))
                .AddScoped<IWeatherForecastApplicationService, WeatherForecastApplicationService>()
                .AddScoped<IWeatherForecastService, WeatherForecastService>()
                .AddValidatorsFromAssembly(typeof(RegistrationExtensions).Assembly);

            return services;
        }
    }
}
