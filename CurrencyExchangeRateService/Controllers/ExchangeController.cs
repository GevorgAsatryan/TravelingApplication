using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CurrencyExchangeRateService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeController : ControllerBase
    {
        [HttpPost]
        public async Task<string> Exchange([FromBody] ExchangeDetails details)
        {
            string apiKey = "JtAeBGKoEDVw3Yxce4rg0wE4U5lyq6QyXfJ5XgoPwL6nlHOjvUpOpGG9nAIIIyWT";

            string url = $"https://api.unirateapi.com/api/convert?api_key={apiKey}&from={details.currency}&to={details.preferredCurrency}&amount={details.amount}";

            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    return "Incorrect Currency";
                }

                var content = await response.Content.ReadAsStringAsync();
                Exchange deserializedData = JsonSerializer.Deserialize<Exchange>(content);

                return deserializedData.result + " " + details.preferredCurrency;
            }
        }
    }
}
