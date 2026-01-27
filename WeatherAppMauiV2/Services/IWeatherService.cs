using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAppMauiV2.Models;

namespace WeatherAppMauiV2.Services
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeatherAsync(string cityName);
        Task<List<DailyForecast>> GetForecastAsync(string cityName);
    }
}
