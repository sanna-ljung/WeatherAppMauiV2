using WeatherAppMauiV2.Views;

namespace WeatherAppMauiV2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(HourlyForecastDetailPage), typeof(HourlyForecastDetailPage));
        }
    }
}
