using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherAppMauiV2.Models;
using WeatherAppMauiV2.Services;

namespace WeatherAppMauiV2.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IWeatherService _weatherService;

        [ObservableProperty]
        private string _cityName = "Stockholm";

        [ObservableProperty]
        private double _temperature;

        [ObservableProperty]
        private string _weatherDescription = "Sunny";

        [ObservableProperty]
        private int _humidity = 65;

        [ObservableProperty]
        private double _windSpeed = 8;

        [ObservableProperty]
        private double _feelsLike = 20;

        [ObservableProperty]
        private string _weatherIcon = "☀️";

        [ObservableProperty]
        private bool _isLoading;

        public ObservableCollection<HourlyForecast> HourlyForecasts { get; set; }

        public MainViewModel(IWeatherService weatherService)
        {
            _weatherService = weatherService;
            HourlyForecasts = new ObservableCollection<HourlyForecast>();

            // Ladda väder direkt
            _ = LoadWeatherAsync();
        }

        // RelayCommand genererar automatiskt ICommand properties
        [RelayCommand]
        private async Task LoadWeatherAsync()
        {
            IsLoading = true;

            try
            {
                var weather = await _weatherService.GetWeatherAsync(CityName);

                // Uppdatera aktuellt väder
                Temperature = weather.Temperature;
                WeatherDescription = weather.WeatherDescription;
                Humidity = weather.Humidity;
                WindSpeed = weather.WindSpeed;
                FeelsLike = weather.Temperature - 2;
                WeatherIcon = GetWeatherEmoji(weather.WeatherCode);

                // Ladda 24-timmars prognos
                LoadHourlyForecast();
            }
            catch (Exception ex)
            {
                await ShowErrorAsync("Fel", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand(CanExecute = nameof(CanSearch))]
        private async Task SearchWeatherAsync(string city)
        {
            CityName = city;
            await LoadWeatherAsync();
        }

        private bool CanSearch(string city)
        {
            return !string.IsNullOrWhiteSpace(city) && !IsLoading;
        }

        private void LoadHourlyForecast()
        {
            HourlyForecasts.Clear();

            var random = new Random();
            for (int i = 0; i < 24; i++)
            {
                HourlyForecasts.Add(new HourlyForecast
                {
                    Time = DateTime.Now.AddHours(i).ToString("HH:mm"),
                    Temperature = Temperature + random.Next(-3, 4),
                    WeatherCode = random.Next(0, 4),
                    Icon = GetWeatherEmoji(random.Next(0, 4))
                });
            }
        }

        private static string GetWeatherEmoji(int weatherCode)
        {
            return weatherCode switch
            {
                0 => "☀️",  // Clear sky
                1 => "🌤️",  // Mainly clear
                2 => "⛅",  // Partly cloudy
                3 => "☁️",  // Overcast
                45 => "🌫️", // Fog
                51 => "🌦️", // Drizzle
                61 => "🌧️", // Rain
                71 => "🌨️", // Snow
                95 => "⛈️", // Thunderstorm
                _ => "☀️"
            };
        }

        private static async Task ShowErrorAsync(string title, string message)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            }
        }
    }
}
