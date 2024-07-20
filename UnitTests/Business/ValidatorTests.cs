using Business.Dtos;
using Business.Validators;
using FluentValidation.TestHelper;

namespace UnitTests.Business;

public class ValidatorTests
{
    private readonly AddWeatherForecastDtoValidator _validator = new();

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-61)]
    [InlineData(61)]
    [InlineData(int.MaxValue)]
    public void AddWeatherForecastDtoValidator_Have_Error_When_Temperature_Is_Invalid(int temperature)
    {
        // Arrange
        var dto = new AddWeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            Temperature = temperature
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Temperature);
        result.ShouldNotHaveValidationErrorFor(p => p.Date);
    }

    [Theory]
    [InlineData(-60)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(60)]
    public void AddWeatherForecastDtoValidator_Successful_When_Dto_Is_Valid(int temperature)
    {
        // Arrange
        var dto = new AddWeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            Temperature = temperature
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.Temperature);
        result.ShouldNotHaveValidationErrorFor(p => p.Date);
    }

    [Fact]
    public void AddWeatherForecastDtoValidator_Have_Error_When_Date_Is_Invalid()
    {
        // Arrange
        var dto = new AddWeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
            Temperature = 5
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Date);
        result.ShouldNotHaveValidationErrorFor(p => p.Temperature);
    }
}