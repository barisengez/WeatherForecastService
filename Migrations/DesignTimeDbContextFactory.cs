using DataAccess.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Migrations
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WeatherForecastDbContext>
    {
        public WeatherForecastDbContext CreateDbContext(string[] args)
        {
            var builder = DbContextHelper.GetDefaultDbContextOptionsBuilder();

            return new WeatherForecastDbContext(builder.Options);
        }
    }
}
