using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Consumes("text/plain")]
    public class WeatherController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public WeatherController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] string cityAndCountry)
        {
            var cachedResult = await _distributedCache.GetStringAsync(cityAndCountry);

            if (cachedResult != null)
            {
                Root weatherData = JsonSerializer.Deserialize<Root>(cachedResult);

                string futureForcast = "";

                foreach (var day in weatherData.days)
                {
                    futureForcast += $"Date: {day.datetime}\n\n" +
                          $"Temperature: {day.tempmax}°C / {day.tempmin}°C\n" +
                          $"Humidity: {day.humidity}%\n" +
                          $"Conditions: {day.conditions}\n" +
                          $"Sunrise: {day.sunrise}\n" +
                          $"Sunset: {day.sunset}\n\n";
                }
                futureForcast = $"Weather in {cityAndCountry}\n\n" + futureForcast;

                return futureForcast;
            }

            string dataResult = await GetWeatherData(cityAndCountry);

            return dataResult;
        }

        public async Task<string> GetWeatherData(string cityAndCountry)
        {
            string apiKey = "FM4PSAFFWRL25LL595KN6NV2V";
            string location = $"{cityAndCountry}";
            string url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{location}?unitGroup=metric&key={apiKey}&contentType=json";
            HttpClient httpClient = new HttpClient();
            string result = "";
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                Root weatherData = JsonSerializer.Deserialize<Root>(content);
                var value = System.Text.Encoding.UTF8.GetBytes(content);
                await _distributedCache.SetAsync(cityAndCountry, value);

                foreach (var day in weatherData.days)
                {
                    result += $"Date: {day.datetime}\n\n" +
                              $"Temperature: {day.tempmax}°C / {day.tempmin}°C\n" +
                              $"Humidity: {day.humidity}%\n" +
                              $"Conditions: {day.conditions}\n" +
                              $"Sunrise: {day.sunrise}\n" +
                              $"Sunset: {day.sunset}\n\n";
                }
                result = $"Weather in {cityAndCountry}\n\n" + result;
            }
            catch (Exception ex)
            {
                return $"There is no weather data for this city";
            }
            return result;
        }
    }

    public class Alert
    {
        public string @event { get; set; }
        public string headline { get; set; }
        public DateTime ends { get; set; }
        public int endsEpoch { get; set; }
        public DateTime onset { get; set; }
        public int onsetEpoch { get; set; }
        public string id { get; set; }
        public string language { get; set; }
        public string link { get; set; }
        public string description { get; set; }
    }

    public class AT416
    {
        public double distance { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int useCount { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int quality { get; set; }
        public double contribution { get; set; }
    }

    public class AT964
    {
        public double distance { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int useCount { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int quality { get; set; }
        public double contribution { get; set; }
    }

    public class AV731
    {
        public double distance { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int useCount { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int quality { get; set; }
        public double contribution { get; set; }
    }

    public class CurrentConditions
    {
        public string datetime { get; set; }
        public int datetimeEpoch { get; set; }
        public double temp { get; set; }
        public double feelslike { get; set; }
        public double humidity { get; set; }
        public double dew { get; set; }
        public double precip { get; set; }
        public double precipprob { get; set; }
        public double snow { get; set; }
        public double snowdepth { get; set; }
        public object preciptype { get; set; }
        public double windgust { get; set; }
        public double windspeed { get; set; }
        public double winddir { get; set; }
        public double pressure { get; set; }
        public double visibility { get; set; }
        public double cloudcover { get; set; }
        public double solarradiation { get; set; }
        public double solarenergy { get; set; }
        public double uvindex { get; set; }
        public string conditions { get; set; }
        public string icon { get; set; }
        public List<string> stations { get; set; }
        public string source { get; set; }
        public string sunrise { get; set; }
        public int sunriseEpoch { get; set; }
        public string sunset { get; set; }
        public int sunsetEpoch { get; set; }
        public double moonphase { get; set; }
    }

    public class Day
    {
        public string datetime { get; set; }
        public int datetimeEpoch { get; set; }
        public double tempmax { get; set; }
        public double tempmin { get; set; }
        public double temp { get; set; }
        public double feelslikemax { get; set; }
        public double feelslikemin { get; set; }
        public double feelslike { get; set; }
        public double dew { get; set; }
        public double humidity { get; set; }
        public double precip { get; set; }
        public double precipprob { get; set; }
        public double precipcover { get; set; }
        public List<string> preciptype { get; set; }
        public double snow { get; set; }
        public double snowdepth { get; set; }
        public double windgust { get; set; }
        public double windspeed { get; set; }
        public double winddir { get; set; }
        public double pressure { get; set; }
        public double cloudcover { get; set; }
        public double visibility { get; set; }
        public double solarradiation { get; set; }
        public double solarenergy { get; set; }
        public double uvindex { get; set; }
        public double severerisk { get; set; }
        public string sunrise { get; set; }
        public int sunriseEpoch { get; set; }
        public string sunset { get; set; }
        public int sunsetEpoch { get; set; }
        public double moonphase { get; set; }
        public string conditions { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public List<string> stations { get; set; }
        public string source { get; set; }
        public List<Hour> hours { get; set; }
        public double? tzoffset { get; set; }
    }

    public class Hour
    {
        public string datetime { get; set; }
        public int datetimeEpoch { get; set; }
        public double temp { get; set; }
        public double feelslike { get; set; }
        public double humidity { get; set; }
        public double dew { get; set; }
        public double precip { get; set; }
        public double precipprob { get; set; }
        public double snow { get; set; }
        public double snowdepth { get; set; }
        public List<string> preciptype { get; set; }
        public double windgust { get; set; }
        public double windspeed { get; set; }
        public double winddir { get; set; }
        public double pressure { get; set; }
        public double visibility { get; set; }
        public double cloudcover { get; set; }
        public double solarradiation { get; set; }
        public double solarenergy { get; set; }
        public double uvindex { get; set; }
        public double severerisk { get; set; }
        public string conditions { get; set; }
        public string icon { get; set; }
        public List<string> stations { get; set; }
        public string source { get; set; }
        public double? tzoffset { get; set; }
    }

    public class LIRA
    {
        public double distance { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int useCount { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int quality { get; set; }
        public double contribution { get; set; }
    }

    public class LIRE
    {
        public double distance { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int useCount { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int quality { get; set; }
        public double contribution { get; set; }
    }

    public class LIRF
    {
        public double distance { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int useCount { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int quality { get; set; }
        public double contribution { get; set; }
    }

    public class LIRU
    {
        public double distance { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int useCount { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int quality { get; set; }
        public double contribution { get; set; }
    }

    public class Root
    {
        public int queryCost { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string resolvedAddress { get; set; }
        public string address { get; set; }
        public string timezone { get; set; }
        public double tzoffset { get; set; }
        public string description { get; set; }
        public List<Day> days { get; set; }
        public List<Alert> alerts { get; set; }
        public Stations stations { get; set; }
        public CurrentConditions currentConditions { get; set; }
    }

    public class Stations
    {
        public LIRF LIRF { get; set; }
        public LIRE LIRE { get; set; }
        public LIRU LIRU { get; set; }
        public AV731 AV731 { get; set; }
        public AT416 AT416 { get; set; }
        public LIRA LIRA { get; set; }
        public AT964 AT964 { get; set; }
    }
}

