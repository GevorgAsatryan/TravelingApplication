using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AdditionalInformationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InformationController : ControllerBase
    {
        [HttpPost]
        public async Task<string> Post([FromBody] string country)
        {
            var url = $"https://countries.dev/name/{country}";

            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    return "Not Found";
                }
                var content = await response.Content.ReadAsStringAsync();
                var informationDetails = JsonSerializer.Deserialize<Root[]>(content);

                return

                $"{informationDetails[0].name} ({informationDetails[0].nativeName})\n\n" +
                $"Capital: {informationDetails[0].capital}\n" +
                $"Region: {informationDetails[0].region} / {informationDetails[0].subregion}\n" +
                $"Population: {informationDetails[0].population:N0}\n" +
                $"Area: {informationDetails[0].area:N0} km?\n" +
                $"Population Density: {informationDetails[0].populationDensity:N2} people/km?\n\n" +
                
                $"Currency:\n" +
                $"{string.Join(", ", informationDetails[0].currencies.Select(c =>
                    $"{c.name} ({c.code}) {c.symbol}"))}\n\n" +
                
                $"Languages:\n" +
                $"{string.Join(", ", informationDetails[0].languages.Select(l =>
                    $"{l.name} ({l.nativeName})"))}\n\n" +
                
                $"Timezones:\n" +
                $"{string.Join(", ", informationDetails[0].timezones)}\n\n" +
                
                $"Calling Code: +{string.Join(", +", informationDetails[0].callingCodes)}\n" +
                $"Domain: {string.Join(", ", informationDetails[0].topLevelDomain)}\n\n" +
                
                $"Coordinates: {informationDetails[0].latlng[0]}, {informationDetails[0].latlng[1]}\n" +
                $"Google Maps: https://www.google.com/maps/@{informationDetails[0].latlng[0]},{informationDetails[0].latlng[1]},6z\n\n" +
                
                $"Flag: {informationDetails[0].flags.svg}\n" +
                $"(Copy the URL and paste it into your browser.)\n\n" +
                
                $"Bordering Countries: {string.Join(", ", informationDetails[0].borders)}\n" +
                $"Independent: {(informationDetails[0].independent ? "Yes" : "No")}\n";
            }
        }
    }


}
