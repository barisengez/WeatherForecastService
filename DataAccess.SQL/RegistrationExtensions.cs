using Core.Repositories;
using DataAccess.SQL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.SQL
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddWeatherForecastDataAccess(
            this IServiceCollection services, IConfigurationManager configurationManager)
        {
            services.AddDbContext<WeatherForecastDbContext>(options =>
                options.UseSqlServer(configurationManager.GetConnectionString("DefaultConnection")));

            services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

            return services;
        }
    }
}
