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
            var url = $"https://restcountries.com/v3.1/name/{country}";

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

                return $"Capital {informationDetails[0].capital[0]}\n" +
                       $"Google Map {informationDetails[0].maps.googleMaps} (Copy the URL and paste it into your browser.)\n" +
                       $"Area {informationDetails[0].area} square kilometre\n" +
                       $"Population {informationDetails[0].population}\n" +
                       $"Flag {informationDetails[0].flags.alt} \n" +
                       $"{informationDetails[0].flags.svg} (Copy the URL and paste it into your browser.)\n";
            }
        }
    }


}
