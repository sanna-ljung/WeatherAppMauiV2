namespace WeatherAppMauiV2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

#if WINDOWS
            if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            {
                // Smartphone-liknande dimensioner
                window.Width = 420;
                window.Height = 915;
            }
#endif

            return window;
        }
    }
}