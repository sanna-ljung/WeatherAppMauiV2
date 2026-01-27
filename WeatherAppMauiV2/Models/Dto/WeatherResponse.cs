using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherAppMauiV2.Models.Dto
{
    public class WeatherResponse
    {
        [JsonPropertyName("current")]
        public CurrentWeather Current { get; set; }

        [JsonPropertyName("daily")]
        public DailyWeather Daily { get; set; }
    }
}
