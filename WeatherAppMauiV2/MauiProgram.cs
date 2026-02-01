using Microsoft.Extensions.Logging;
using WeatherAppMauiV2.Services;
using WeatherAppMauiV2.ViewModels;
using WeatherAppMauiV2.Views;

namespace WeatherAppMauiV2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Fredoka-Bold.ttf", "FredokaBold");
                    fonts.AddFont("Fredoka-SemiBold.ttf", "FredokaSemiBold");
                    fonts.AddFont("Fredoka-Medium.ttf", "FredokaMedium");
                    fonts.AddFont("Fredoka-Light.ttf", "FredokaLight");
                });

            // Registrera services
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IWeatherService, WeatherAPIService>();

            // Registrera ViewModels
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<ForecastDetailViewModel>();
            builder.Services.AddSingleton<FavoriteCitiesViewModel>();

            // Registrera Views
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<HourlyForecastDetailPage>();
            builder.Services.AddSingleton<FavoriteCitiesPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
