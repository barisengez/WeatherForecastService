using DataAccess.SQL;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Helpers;

public class WeatherForecastDbContextFixture : IDisposable
{
    public WeatherForecastDbContextFixture()
    {
        var options = new DbContextOptionsBuilder<WeatherForecastDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        Context = new WeatherForecastDbContext(options);
        Context.Database.EnsureCreated();
    }

    public WeatherForecastDbContext Context { get; }

    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }

    public void ClearDatabase()
    {
        Context.WeatherForecasts.RemoveRange(Context.WeatherForecasts);
        Context.SaveChanges();
    }
}