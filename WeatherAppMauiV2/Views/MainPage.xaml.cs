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
                    searchBar.Text = string.Empty; // Rensa sökfältet
                }
            }
        }

    }
}
