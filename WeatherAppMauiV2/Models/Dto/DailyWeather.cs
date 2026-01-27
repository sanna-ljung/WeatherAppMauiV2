using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherAppMauiV2.Models.Dto
{
    public class DailyWeather
    {
        [JsonPropertyName("time")]
        public List<string> Time { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public List<double> MaxTemperatures { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double> MinTemperatures { get; set; }

        [JsonPropertyName("weather_code")]
        public List<int> WeatherCodes { get; set; }
    }
}
