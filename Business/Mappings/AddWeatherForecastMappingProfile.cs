using AutoMapper;
using Business.Dtos;
using Core.Entities;

namespace Business.Mappings
{
    public class AddWeatherForecastMappingProfile : Profile
    {
        public AddWeatherForecastMappingProfile()
        {
            CreateMap<AddWeatherForecastDto,WeatherForecast>();
        }
    }
}
