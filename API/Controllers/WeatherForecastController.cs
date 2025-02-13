using System.Net;
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
    /// <response code="201">If the weather forecast is added successfully.</response>
    /// <response code="400">If the weather forecast data is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(WeatherForecastDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(List<ValidationFailure>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddWeatherForecastAsync(AddWeatherForecastDto dto)
    {
        var validationResult = await addWeatherForecastDtoValidator.ValidateAsync(dto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var addedDto = await appService.AddWeatherForecastAsync(dto);

        return Created(string.Empty, addedDto);
    }

    /// <summary>
    ///     Gets a weather forecast by ID.
    /// </summary>
    /// <returns>The weather forecast dto</returns>
    /// <response code="200">If the weather forecast is found.</response>
    /// <response code="404">If the weather forecast is not found.</response>
    [HttpGet("weekly-forecast")]
    [ProducesResponseType(typeof(WeatherForecastDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetWeeklyWeatherForecastAsync()
    {
        var forecasts = await appService.GetWeeklyWeatherForecastAsync();
        if (forecasts.Count == 0) return NotFound();

        return Ok(forecasts);
    }
}