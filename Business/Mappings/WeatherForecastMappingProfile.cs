using AutoMapper;
using Business.Dtos;
using Core.Entities;

namespace Business.Mappings
{
    public class WeatherForecastMappingProfile : Profile
    {
        private static readonly List<(int Min, int Max, TemperatureDescription Description)> TemperatureRanges =
            [
                (-60, 0, TemperatureDescription.Freezing),
                (1, 10, TemperatureDescription.Bracing),
                (11, 15, TemperatureDescription.Chilly),
                (16, 20, TemperatureDescription.Cool),
                (21, 25, TemperatureDescription.Mild),
                (26, 30, TemperatureDescription.Warm),
                (31, 35, TemperatureDescription.Balmy),
                (36, 40, TemperatureDescription.Hot),
                (41, 45, TemperatureDescription.Sweltering),
                (46, int.MaxValue, TemperatureDescription.Scorching)
            ];

        public WeatherForecastMappingProfile()
        {
            CreateMap<WeatherForecast, WeatherForecastDto>()
                .ForMember(dest => dest.HumanFriendlyTemperatureDescription,
                    opt => opt.MapFrom(src => MapTemperatureToDescription(src.Temperature)));
        }

        private static TemperatureDescription MapTemperatureToDescription(int temperature)
        {
            return TemperatureRanges.Where(range => temperature >= range.Min && temperature <= range.Max)
                .Select(range => range.Description).FirstOrDefault();
        }
    }
}
