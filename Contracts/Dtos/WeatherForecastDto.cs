namespace Contracts.Models
{
    public class WeatherForecastDto
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public string TemperatureUnit { get; set; }
        public string Summary { get; set; }
    }
}
