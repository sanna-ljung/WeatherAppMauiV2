using WeatherAppMauiV2.ViewModels;

namespace WeatherAppMauiV2.Views;

public partial class FavoriteCitiesPage : ContentPage
{
    public FavoriteCitiesPage(FavoriteCitiesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    private async void OnCitySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is CityItem city)
        {
            // Navigera tillbaka till MainPage med parameter
            await Shell.Current.GoToAsync($"//MainPage?city={city.Name}");

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}