using WeatherAppMauiV2.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WeatherAppMauiV2.ViewModels
{
    [QueryProperty(nameof(Forecast), "Forecast")]
    public partial class ForecastDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private HourlyForecast _forecast;

        [ObservableProperty]
        private string _weatherDescription = "";

        [ObservableProperty]
        private int _precipitationChance = 20;

        partial void OnForecastChanged(HourlyForecast value)
        {
            if (value != null)
            {
                WeatherDescription = GetWeatherDescription(value.WeatherCode);
                PrecipitationChance = new Random().Next(0, 100); // Mock data
            }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        private string GetWeatherDescription(int weatherCode)
        {
            return weatherCode switch
            {
                0 => "Klar himmel",
                1 => "Mestadels klart",
                2 => "Delvis molnigt",
                3 => "Mulet",
                45 => "Dimma",
                51 => "Lätt duggregn",
                61 => "Regn",
                71 => "Snö",
                95 => "Åska",
                _ => "Okänt"
            };
        }
    }
}
