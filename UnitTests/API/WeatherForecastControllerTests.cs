using API.Controllers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Business.Dtos;
using Business.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnitTests.Helpers;

namespace UnitTests.API;

public class WeatherForecastControllerTests
{
    private readonly IFixture _fixture = new Fixture()
        .Customize(new CompositeCustomization(new AutoMoqCustomization(), new DateOnlyFixtureCustomization()));

    [Fact]
    public async Task AddWeatherForecast_Calls_Service_On_Valid_Input()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<AddWeatherForecastDto>>();
        var mockService = new Mock<IWeatherForecastApplicationService>();
        var controller = new WeatherForecastController(mockService.Object, mockValidator.Object);
        var validDto = _fixture.Create<AddWeatherForecastDto>();
        mockValidator.Setup(v => v.ValidateAsync(validDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult()).Verifiable();
        mockService.Setup(p => p.AddWeatherForecastAsync(validDto)).Returns(Task.CompletedTask).Verifiable();

        // Act
        var result = await controller.AddWeatherForecast(validDto);

        // Assert
        Assert.IsType<CreatedResult>(result);
        mockValidator.Verify();
        mockService.Verify();
    }

    [Fact]
    public async Task AddWeatherForecast_Returns_BadRequest_On_Invalid_Input()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<AddWeatherForecastDto>>();
        var mockService = new Mock<IWeatherForecastApplicationService>();
        var controller = new WeatherForecastController(mockService.Object, mockValidator.Object);
        var invalidDto = _fixture.Create<AddWeatherForecastDto>();
        var validationFailures = new List<ValidationFailure> { new("Property", "Error message") };
        mockValidator.Setup(v => v.ValidateAsync(invalidDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act
        var result = await controller.AddWeatherForecast(invalidDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        mockValidator.Verify();
        Assert.Equal(validationFailures, badRequestResult.Value);
    }

    [Fact]
    public async Task GetWeeklyWeatherForecast_Returns_Successful_List()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<AddWeatherForecastDto>>();
        var mockService = new Mock<IWeatherForecastApplicationService>();
        var forecasts = _fixture.CreateMany<WeatherForecastDto>(3).ToList();
        mockService.Setup(service => service.GetWeeklyWeatherForecastAsync()).ReturnsAsync(forecasts);
        var controller = new WeatherForecastController(mockService.Object, mockValidator.Object);

        // Act
        var result = await controller.GetWeeklyWeatherForecast();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<WeatherForecastDto>>(okResult.Value);
        Assert.Equal(3, returnValue.Count);
    }

    [Fact]
    public async Task GetWeeklyWeatherForecast_Returns_404NotFound_When_No_WeatherForecasts_Available()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<AddWeatherForecastDto>>();
        var mockService = new Mock<IWeatherForecastApplicationService>();
        var forecasts = new List<WeatherForecastDto>();
        mockService.Setup(service => service.GetWeeklyWeatherForecastAsync()).ReturnsAsync(forecasts);
        var controller = new WeatherForecastController(mockService.Object, mockValidator.Object);

        // Act
        var result = await controller.GetWeeklyWeatherForecast();

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}