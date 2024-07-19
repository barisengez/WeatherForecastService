using Core.Entities;
using DataAccess.SQL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.SQL
{
    public class WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) : DbContext(options)
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new WeatherForecastConfiguration());
        }
    }
}
