using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Business.Dtos;
using Business.Mappings;
using Business.Services;
using Core.Entities;
using Core.Services;
using Moq;
using UnitTests.Helpers;

namespace UnitTests.Business;

public class WeatherForecastApplicationServiceTests
{
    private readonly IFixture _fixture = new Fixture()
        .Customize(new CompositeCustomization(new AutoMoqCustomization(), new DateOnlyFixtureCustomization()));

    public WeatherForecastApplicationServiceTests()
    {
        _fixture.Customize<IMapper>(c =>
            c.FromFactory(new MapperConfiguration(m =>
            {
                m.AddProfile<AddWeatherForecastMappingProfile>();
                m.AddProfile<WeatherForecastMappingProfile>();
            }).CreateMapper));
    }

    [Fact]
    public async Task AddWeatherForecastAsync_Calls_AddWeatherForecastAsync()
    {
        // Arrange
        var weatherForecastServiceMock = new Mock<IWeatherForecastService>();
        var service = new WeatherForecastApplicationService(weatherForecastServiceMock.Object, _fixture.Create<IMapper>());
        var dto = _fixture.Create<AddWeatherForecastDto>();

        weatherForecastServiceMock.Setup(m => m.AddWeatherForecastAsync(
                It.Is<WeatherForecast>(k => k.Date == dto.Date && k.Temperature == dto.Temperature)))
            .Returns(Task.CompletedTask).Verifiable();

        // Act
        await service.AddWeatherForecastAsync(dto);

        // Assert
        weatherForecastServiceMock.Verify();
    }

    [Fact]
    public async Task GetWeeklyWeatherForecastAsync_Returns_Data_Providing_Day_Limit()
    {
        // Arrange
        var weatherForecastServiceMock = new Mock<IWeatherForecastService>();
        var weatherForecasts = _fixture.CreateMany<WeatherForecast>(3).ToList();

        weatherForecastServiceMock
            .Setup(service => service.GetWeatherForecastAsync(It.IsAny<DateOnly>(), 7))
            .ReturnsAsync(weatherForecasts);

        var service = new WeatherForecastApplicationService(weatherForecastServiceMock.Object, _fixture.Create<IMapper>());

        // Act
        var result = await service.GetWeeklyWeatherForecastAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(weatherForecasts.Count, result.Count);
        for (var i = 0; i < weatherForecasts.Count; i++)
        {
            Assert.Equal(weatherForecasts[i].Date, result[i].Date);
            Assert.Equal(weatherForecasts[i].Temperature, result[i].Temperature);
        }
    }
}