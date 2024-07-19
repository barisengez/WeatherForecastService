using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.SQL.Configurations;

public class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(w => w.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.Temperature).IsRequired();
    }
}