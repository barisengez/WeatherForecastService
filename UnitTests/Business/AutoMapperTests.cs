using AutoMapper;
using Business.Mappings;

namespace UnitTests.Business;

public class AutoMapperTests
{
    [Fact]
    public void AddWeatherForecastMappingTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AddWeatherForecastMappingProfile>());
        config.AssertConfigurationIsValid();
    }

    [Fact]
    public void WeatherForecastMappingTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<WeatherForecastMappingProfile>());
        config.AssertConfigurationIsValid();
    }
}