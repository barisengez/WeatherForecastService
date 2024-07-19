using Core.Repositories;
using DataAccess.SQL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.SQL;

public static class RegistrationExtensions
{
    public static IServiceCollection AddWeatherForecastDataAccess(this IServiceCollection services)
    {
        var connectionString = DbContextHelper.GetDefaultConnectionString();

        services.AddDbContext<WeatherForecastDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}