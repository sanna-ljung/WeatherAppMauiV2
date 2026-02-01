using WeatherAppMauiV2.Models;
using WeatherAppMauiV2.ViewModels;

namespace WeatherAppMauiV2
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            if (sender is SearchBar searchBar &&
                BindingContext is MainViewModel viewModel)
            {
                var city = searchBar.Text;
                if (!string.IsNullOrWhiteSpace(city))
                {
                    await viewModel.SearchWeatherCommand.ExecuteAsync(city);
                    searchBar.Text = string.Empty;
                }
            }
        }
        private async void OnForecastSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is HourlyForecast forecast &&
                BindingContext is MainViewModel viewModel)
            {
                await viewModel.NavigateToForecastDetailCommand.ExecuteAsync(forecast);

                // Avmarkera
                ((CollectionView)sender).SelectedItem = null;
            }
        }

    }
}
