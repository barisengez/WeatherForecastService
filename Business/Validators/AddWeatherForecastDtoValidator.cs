﻿using Business.Dtos;
using FluentValidation;

namespace Business.Validators;

public class AddWeatherForecastDtoValidator : AbstractValidator<AddWeatherForecastDto>
{
    public AddWeatherForecastDtoValidator()
    {
        RuleFor(x => x.Temperature)
            .InclusiveBetween(-60, 60)
            .WithMessage("Temperature must be between -60 and 60 degrees.");

        RuleFor(x => x.Date)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Input forecasts cannot be in the past.");
    }
}