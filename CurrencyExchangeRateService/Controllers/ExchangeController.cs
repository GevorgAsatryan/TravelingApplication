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

        private static int attempt = 0;

        public ExchangeController(IOptions<ApiKeys> apiKeys)
        {
            _apiKeys = apiKeys.Value;
        }
        [HttpGet]
        public async Task<string> Exchange([FromQuery] string json)
        {
            //attempt++;

            //Console.WriteLine($"Service attempt: {attempt}");

            //if (attempt <= 2)
            //{
            //    throw new Exception();
            //}

            string apiKey = _apiKeys.Currency;

            var details = JsonSerializer.Deserialize<ExchangeDetails>(json);

            string url = $"https://api.unirateapi.com/api/convert?api_key={apiKey}&from={details?.currency}&to={details?.preferredCurrency}&amount={details?.amount}";

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
