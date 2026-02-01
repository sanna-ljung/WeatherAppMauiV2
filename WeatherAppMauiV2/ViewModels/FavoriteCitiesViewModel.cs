using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAppMauiV2.ViewModels
{
    public partial class FavoriteCitiesViewModel : ObservableObject
    {
        public ObservableCollection<CityItem> Cities { get; set; }

        public FavoriteCitiesViewModel()
        {
            Cities = new ObservableCollection<CityItem>
            {
                new CityItem { Name = "Stockholm", Country = "Sverige", Flag = "🇸🇪" },
                new CityItem { Name = "Göteborg", Country = "Sverige", Flag = "🇸🇪" },
                new CityItem { Name = "Malmö", Country = "Sverige", Flag = "🇸🇪" },
                new CityItem { Name = "London", Country = "Storbritannien", Flag = "🇬🇧" },
                new CityItem { Name = "Paris", Country = "Frankrike", Flag = "🇫🇷" },
                new CityItem { Name = "New York", Country = "USA", Flag = "🇺🇸" },
                new CityItem { Name = "Tokyo", Country = "Japan", Flag = "🇯🇵" },
                new CityItem { Name = "Sydney", Country = "Australien", Flag = "🇦🇺" },
            };
        }
    }
}
