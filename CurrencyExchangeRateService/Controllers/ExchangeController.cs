using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CurrencyExchangeRateService.Configuration;
using Microsoft.Extensions.Options;

namespace CurrencyExchangeRateService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeController : ControllerBase
    {
        private readonly ApiKeys _apiKeys;

        public ExchangeController(IOptions<ApiKeys> apiKeys)
        {
            _apiKeys = apiKeys.Value;
        }
        [HttpPost]
        public async Task<string> Exchange([FromBody] ExchangeDetails details)
        {
            string apiKey = _apiKeys.Currency;

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
