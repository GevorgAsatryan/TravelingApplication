using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FoodInformationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodInformationController : ControllerBase
    {
        [HttpPost]
        public async Task<string> Post([FromBody] string country)
        {
            var url = $"https://www.themealdb.com/api/json/v1/1/filter.php?a={country}";

            using(HttpClient httpClient = new HttpClient())
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
                var details = JsonSerializer.Deserialize<Root>(content);

                return
                $"Dishes:\n" +
                string.Join("\n", details.meals.Select(m => $"- {m.strMeal} \n {m.strMealThumb} (Copy and paste this URL in your browser.)"));

            }

        }
    }
}
