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
        var validDtoToAdd = _fixture.Create<AddWeatherForecastDto>();
        var addedDto = _fixture.Build<WeatherForecastDto>()
            .With(p => p.Date, validDtoToAdd.Date)
            .With(p => p.Temperature, validDtoToAdd.Temperature)
            .Create();
        mockValidator.Setup(v => v.ValidateAsync(validDtoToAdd, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult()).Verifiable();
        mockService.Setup(p => p.AddWeatherForecastAsync(validDtoToAdd))
            .ReturnsAsync(addedDto)
            .Verifiable();

        // Act
        var result = await controller.AddWeatherForecastAsync(validDtoToAdd);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        var returnValue = Assert.IsType<WeatherForecastDto>(createdResult.Value);
        Assert.Equal(validDtoToAdd.Date, returnValue.Date);
        Assert.Equal(validDtoToAdd.Temperature, returnValue.Temperature);
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
        var result = await controller.AddWeatherForecastAsync(invalidDto);

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
        var result = await controller.GetWeeklyWeatherForecastAsync();

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
        var result = await controller.GetWeeklyWeatherForecastAsync();

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}