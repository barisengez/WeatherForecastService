using Business.Dtos;
using Business.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(
    IWeatherForecastApplicationService appService,
    IValidator<AddWeatherForecastDto> addWeatherForecastDtoValidator) : ControllerBase
{
    /// <summary>
    ///     Adds a new weather forecast.
    /// </summary>
    /// <param name="dto">The weather forecast dto</param>
    /// <returns>Result of the operation.</returns>
    /// <response code="200">If the weather forecast is added successfully.</response>
    /// <response code="400">If the weather forecast data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(void), 200)]
    [ProducesResponseType(typeof(List<ValidationFailure>), 400)]
    public async Task<IActionResult> AddWeatherForecast(AddWeatherForecastDto dto)
    {
        var validationResult = await addWeatherForecastDtoValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        await appService.AddWeatherForecastAsync(dto);

        return Ok();
    }

    /// <summary>
    ///     Gets a weather forecast by ID.
    /// </summary>
    /// <returns>The weather forecast dto</returns>
    /// <response code="200">If the weather forecast is found.</response>
    /// <response code="404">If the weather forecast is not found.</response>
    [HttpGet("weekly-forecast")]
    [ProducesResponseType(typeof(WeatherForecastDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetWeeklyWeatherForecast()
    {
        var forecasts = await appService.GetWeeklyWeatherForecastAsync();
        if (forecasts.Count == 0) return NotFound();
        return Ok(forecasts);
    }
}