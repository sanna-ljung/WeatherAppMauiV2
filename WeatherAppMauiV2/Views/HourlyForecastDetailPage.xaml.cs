using WeatherAppMauiV2.ViewModels;

namespace WeatherAppMauiV2.Views;

public partial class HourlyForecastDetailPage : ContentPage
{
	public HourlyForecastDetailPage(ForecastDetailViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}