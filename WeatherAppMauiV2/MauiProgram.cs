using Microsoft.Extensions.Logging;
using WeatherAppMauiV2.Services;
using WeatherAppMauiV2.ViewModels;

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
            builder.Services.AddSingleton<IWeatherService, WeatherAPIService>();

            // Registrera ViewModels
            builder.Services.AddSingleton<MainViewModel>();

            // Registrera Views
            builder.Services.AddSingleton<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
