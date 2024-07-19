using Business.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(IWeatherForecastApplicationService appService) : ControllerBase
    {
        /// <summary>
        /// Adds a new weather forecast.
        /// </summary>
        /// <param name="dto">The weather forecast dto</param>
        /// <returns>Result of the operation.</returns>
        /// <response code="200">If the weather forecast is added successfully.</response>
        /// <response code="400">If the weather forecast data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> AddWeatherForecast([FromBody] AddWeatherForecastDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await appService.AddWeatherForecastAsync(dto);

            return Ok();
        }

        /// <summary>
        /// Gets a weather forecast by ID.
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
            if (forecasts.Count == 0)
            {
                return NotFound();
            }
            return Ok(forecasts);
        }
    }
}
