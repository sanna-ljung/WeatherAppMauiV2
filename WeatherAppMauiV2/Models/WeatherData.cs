using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAppMauiV2.Models
{
    public class WeatherData
    {
        public string CityName { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int WeatherCode { get; set; }
        public string WeatherDescription { get; set; }
        public DateTime DateTime { get; set; }
    }
}
