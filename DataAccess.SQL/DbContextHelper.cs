using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.SQL;

public static class DbContextHelper
{
    public static string GetDefaultConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var csBuilder = new SqlConnectionStringBuilder(connectionString);

        var overrideServerWithValue = Environment.GetEnvironmentVariable("ConnectionStrings__OverrideServerWith");
        if (!string.IsNullOrEmpty(overrideServerWithValue)) csBuilder["Server"] = overrideServerWithValue;

        return csBuilder.ConnectionString;
    }

    public static DbContextOptionsBuilder<WeatherForecastDbContext> GetDbContextOptionsBuilder()
    {
        var builder = new DbContextOptionsBuilder<WeatherForecastDbContext>();
        var connectionString = GetDefaultConnectionString();
        builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Migrations"));

        return builder;
    }
}