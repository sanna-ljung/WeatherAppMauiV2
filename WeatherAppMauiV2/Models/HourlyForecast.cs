namespace WeatherAppMauiV2.Models
{
    public class HourlyForecast
    {
        public string Time { get; set; }
        public double Temperature { get; set; }
        public int WeatherCode { get; set; }
        public string Icon { get; set; }
    }
}
