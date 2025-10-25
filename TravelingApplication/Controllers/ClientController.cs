using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TravelingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        [HttpGet("Weather")]
        public async Task<string> Get([FromQuery(Name = "Where would you like to travel (City)")] string city, [FromQuery(Name = "Country")] string country)
        {
            string chosenCityAndCountry = $"{city},{country}";
            string url = "https://localhost:7028";
            string endpoint = "/Weather";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                chosenCityAndCountry = $"\"{city},{country}\"";
                var responseContent = new StringContent(chosenCityAndCountry, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpoint, responseContent);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {

                }
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }

        }

        [HttpGet("Login")]
        public async Task<string> Login([Required][FromQuery(Name = "Name")] string name, [Required][EmailAddress(ErrorMessage = "Invalid email address")][FromQuery(Name = "Email")] string email)
        {
            string username = name;
            string userEmail = email;

            var user = new User(name, email);

            return $"Username {user.Name}, Email {user.Email}";
        }
    }
}
