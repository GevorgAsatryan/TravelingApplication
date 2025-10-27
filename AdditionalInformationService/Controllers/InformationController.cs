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
                //var informationDetails = JsonSerializer.Deserialize<Root>(content);

                //return $"Capital {informationDetails.capital}/n" +
                //       $"Flag {informationDetails.flag}/n" +
                //       $"Languages {informationDetails.languages}/n" +
                //       $"Area {informationDetails.area}/n";
                return content;
            }
        }
    }


}
