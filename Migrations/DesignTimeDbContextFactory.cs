using DataAccess.SQL;
using Microsoft.EntityFrameworkCore.Design;

namespace Migrations;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WeatherForecastDbContext>
{
    public WeatherForecastDbContext CreateDbContext(string[] args)
    {
        var builder = DbContextHelper.GetDbContextOptionsBuilder();

        return new WeatherForecastDbContext(builder.Options);
    }
}