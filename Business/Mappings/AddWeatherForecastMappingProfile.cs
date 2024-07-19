using AutoMapper;
using Business.Dtos;
using Core.Entities;

namespace Business.Mappings;

public class AddWeatherForecastMappingProfile : Profile
{
    public AddWeatherForecastMappingProfile()
    {
        CreateMap<AddWeatherForecastDto, WeatherForecast>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}