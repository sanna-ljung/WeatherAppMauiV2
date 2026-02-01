using System.Text.Json;
using WeatherAppMauiV2.Models;
using WeatherAppMauiV2.Models.Dto;

namespace WeatherAppMauiV2.Services
{
    public class WeatherAPIService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private const string WeatherBaseUrl = "https://api.open-meteo.com/v1/forecast";
        private const string GeocodingBaseUrl = "https://geocoding-api.open-meteo.com/v1/search";

        public WeatherAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherData> GetWeatherAsync(string cityName)
        {
            try
            {
                // 1. Hämta koordinater
                var city = await GetCityCoordinatesAsync(cityName);
                if (city == null)
                {
                    throw new Exception($"Kunde inte hitta staden '{cityName}'");
                }

                System.Diagnostics.Debug.WriteLine($"City found: {city.Name}, Lat: {city.Latitude}, Lon: {city.Longitude}");

                // 2. Bygg väder-URL
                var weatherUrl = $"{WeatherBaseUrl}?latitude={city.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                    $"&longitude={city.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                    $"&current=temperature_2m,relative_humidity_2m,weather_code,wind_speed_10m&timezone=auto";

                System.Diagnostics.Debug.WriteLine($"Weather URL: {weatherUrl}");

                // 3. Hämta väderdata
                var weatherResponse = await _httpClient.GetAsync(weatherUrl);

                if (!weatherResponse.IsSuccessStatusCode)
                {
                    var errorContent = await weatherResponse.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Weather API Error: {errorContent}");
                    throw new Exception($"API Error: {weatherResponse.StatusCode}");
                }

                var weatherJson = await weatherResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Weather Response: {weatherJson}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var weather = JsonSerializer.Deserialize<WeatherResponse>(weatherJson, options);

                if (weather?.Current == null)
                {
                    throw new Exception("API returned invalid weather data");
                }

                // 4. Returnera WeatherData
                return new WeatherData
                {
                    CityName = city.Name,
                    Temperature = weather.Current.Temperature,
                    Humidity = weather.Current.Humidity,
                    WindSpeed = weather.Current.WindSpeed,
                    WeatherCode = weather.Current.WeatherCode,
                    WeatherDescription = GetWeatherDescription(weather.Current.WeatherCode),
                    DateTime = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetWeatherAsync Error: {ex.Message}");
                throw;
            }
        }
        private async Task<City> GetCityCoordinatesAsync(string cityName)
        {
            try
            {
                var encodedCity = Uri.EscapeDataString(cityName);
                var geocodingUrl = $"{GeocodingBaseUrl}?name={encodedCity}&count=1&language=sv&format=json";

                System.Diagnostics.Debug.WriteLine($"Geocoding URL: {geocodingUrl}");

                var response = await _httpClient.GetAsync(geocodingUrl);

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"Geocoding failed: {response.StatusCode}");
                    return null;
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Geocoding JSON: {jsonString}");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var geocodingResponse = JsonSerializer.Deserialize<GeocodingResponse>(jsonString, options);

                if (geocodingResponse?.Results == null || !geocodingResponse.Results.Any())
                {
                    System.Diagnostics.Debug.WriteLine("No results in geocoding response");
                    return null;
                }

                var result = geocodingResponse.Results[0];

                return new City
                {
                    Name = result.Name,
                    Country = result.Country ?? "Unknown",
                    Latitude = result.Latitude,
                    Longitude = result.Longitude
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Geocoding Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack: {ex.StackTrace}");
                return null;
            }
        }

        private string GetWeatherDescription(int code)
        {
            return code switch
            {
                0 => "Klar himmel",
                1 => "Mestadels klart",
                2 => "Delvis molnigt",
                3 => "Mulet",
                45 => "Dimma",
                48 => "Rimfrost dimma",
                51 => "Lätt duggregn",
                53 => "Måttligt duggregn",
                55 => "Tätt duggregn",
                61 => "Lätt regn",
                63 => "Måttligt regn",
                65 => "Kraftigt regn",
                71 => "Lätt snöfall",
                73 => "Måttligt snöfall",
                75 => "Kraftigt snöfall",
                77 => "Snögranulat",
                80 => "Lätta regnskurar",
                81 => "Måttliga regnskurar",
                82 => "Kraftiga regnskurar",
                85 => "Lätta snöbyar",
                86 => "Kraftiga snöbyar",
                95 => "Åska",
                96 => "Åska med lätt hagel",
                99 => "Åska med kraftigt hagel",
                _ => "Okänt"
            };
        }
        public Task<List<DailyForecast>> GetForecastAsync(string cityName)
        {
            throw new NotImplementedException();
        }
    }
}
