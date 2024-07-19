namespace Business.Dtos;

public class WeatherForecastDto
{
    public DateOnly Date { get; set; }
    public int Temperature { get; set; }
    public string HumanFriendlyTemperatureDescription { get; set; } = null!;
}