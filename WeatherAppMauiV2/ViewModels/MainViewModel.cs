using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherAppMauiV2.Models;
using WeatherAppMauiV2.Services;
using WeatherAppMauiV2.Views;

namespace WeatherAppMauiV2.ViewModels
{
    [QueryProperty(nameof(SelectedCity), "city")]
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

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private string _selectedCity = "";

        public ObservableCollection<HourlyForecast> HourlyForecasts { get; set; }

        // Constructor
        public MainViewModel(IWeatherService weatherService)
        {
            _weatherService = weatherService;
            HourlyForecasts = new ObservableCollection<HourlyForecast>();

            // ladda när UI är redo
            Task.Run(async () =>
            {
                await Task.Delay(100);
                await LoadWeatherAsync();
            });
        }

        partial void OnSelectedCityChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                CityName = value;
                _ = LoadWeatherAsync();
            }
        }

        // Commands
        [RelayCommand]
        private async Task LoadWeatherAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(CityName))
                {
                    ErrorMessage = "Vänligen ange en stad";
                    return;
                }

                var weather = await _weatherService.GetWeatherAsync(CityName);

                if (weather == null)
                {
                    ErrorMessage = $"Kunde inte hitta väderdata för {CityName}";
                    return;
                }

                // Uppdatera aktuellt väder
                Temperature = weather.Temperature;
                WeatherDescription = weather.WeatherDescription;
                Humidity = weather.Humidity;
                WindSpeed = weather.WindSpeed;
                FeelsLike = weather.Temperature - 2; // Approximation
                WeatherIcon = GetWeatherEmoji(weather.WeatherCode);

                // Ladda 24-timmars prognos
                LoadHourlyForecast();
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "Ingen internetanslutning. Kontrollera din uppkoppling.";
            }
            catch (TaskCanceledException)
            {
                ErrorMessage = "Anropet tog för lång tid. Försök igen.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ett fel uppstod: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Error in LoadWeatherAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SearchWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return;

            CityName = city;
            await LoadWeatherAsync();
        }

        [RelayCommand]
        private async Task NavigateToForecastDetailAsync(HourlyForecast forecast)
        {
            if (forecast == null)
                return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "Forecast", forecast }
            };

            await Shell.Current.GoToAsync(nameof(HourlyForecastDetailPage), navigationParameter);
        }

        // Private Methods
        private void LoadHourlyForecast()
        {
            var forecasts = new List<HourlyForecast>();
            var random = new Random();

            for (int i = 0; i < 24; i++)
            {
                forecasts.Add(new HourlyForecast
                {
                    Time = DateTime.Now.AddHours(i).ToString("HH:mm"),
                    Temperature = Temperature + random.Next(-3, 4),
                    WeatherCode = random.Next(0, 4),
                    Icon = GetWeatherEmoji(random.Next(0, 4))
                });
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                HourlyForecasts.Clear();
                foreach (var forecast in forecasts)
                {
                    HourlyForecasts.Add(forecast);
                }
            });
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
