using System.Data.Common;
using DataAccess.SQL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Migrations
{
    internal static class DbContextHelper
    {
        internal static DbContextOptionsBuilder<WeatherForecastDbContext> GetDefaultDbContextOptionsBuilder()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<WeatherForecastDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var csBuilder = new SqlConnectionStringBuilder(connectionString);

            var overrideServerWithValue = Environment.GetEnvironmentVariable("ConnectionStrings__OverrideServerWith");
            if (!string.IsNullOrEmpty(connectionString))
            {
                csBuilder["Server"] = overrideServerWithValue!;
            }

            Console.WriteLine(csBuilder.ConnectionString);
            builder.UseSqlServer(csBuilder.ConnectionString, b => b.MigrationsAssembly("Migrations"));
            return builder;
        }
    }
}
