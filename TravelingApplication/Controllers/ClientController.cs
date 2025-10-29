using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace TravelingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        [HttpGet("Login")]
        public async Task<string> Login([Required][FromQuery(Name = "Name")] string name, [Required][EmailAddress(ErrorMessage = "Invalid email address")][FromQuery(Name = "Email")] string email)
        {
            string username = name;
            string userEmail = email;

            var user = new User(name, email);
            string secretKey = "this_is_very_very_secret_key_012345";
            string token = TokenManagement.GenerateJwtToken(username, userEmail, secretKey);
            return TokenManagement.ValidateToken(token, secretKey) + " " + $"{token}";
        }

        [HttpGet("Weather")]
        public async Task<string> Get([Required][FromQuery(Name = "Where would you like to travel (City)")] string city, [Required][FromQuery(Name = "Country")] string country)
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
        [HttpGet("Exchange")]
        public async Task<string> Exchange([Required][FromQuery(Name = "Amount")] double amount, [Required][FromQuery(Name = "Convert from")] string baseCurrency, [Required][FromQuery(Name = "To")] string preferredCurrency)
        {
            var details = new
            {
                amount = amount,
                currency = baseCurrency,
                preferredCurrency = preferredCurrency
            };
            string url = "https://localhost:7140";
            string endpoint = "/Exchange";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                var json = JsonSerializer.Serialize(details);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpoint, stringContent);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    return "Not found";
                }
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }

        }
        [HttpGet("Book Hotel")]
        public async Task<string> BookHotel([Required][FromQuery(Name = "Country or city")] string countryOrCity, [Required][DataType(DataType.Date)] DateTime checkin, [Required][DataType(DataType.Date)] DateTime checkout, [Required][Range(0, 30, ErrorMessage = "Adults must be between 0 and 30.")][FromQuery(Name = "Number of Adults")] int adults, [Required][Range(0, 10, ErrorMessage = "Children must be between 0 and 10.")][FromQuery(Name = "Number of children")] int children)
        {
            string url = "https://localhost:7077";
            string endpoint = "Booking";
            var bookingRequest = new
            {
                countryOrCity = countryOrCity,
                checkin = checkin,
                checkout = checkout,
                adults = adults,
                children = children
            };

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                string json = JsonSerializer.Serialize(bookingRequest);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpoint, stringContent);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    return "Not Found";
                }
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
        [HttpGet("Book Flight")]
        public async Task Flight([Required][FromQuery(Name = "Flying from")] string fromCity, [Required][FromQuery(Name = "Flying to")] string toCity, [Required][FromQuery(Name = "Departing")] DateTime departing, [FromQuery(Name = "Returning")] DateTime? returning)
        {
            var flightDetails = new
            {
                fromCity = fromCity,
                toCity = toCity,
                departing = departing,
                returning = returning,
                tripType = ""
            };
            string url = "https://localhost:7044";
            string endpoint = "/Booking";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                var json = JsonSerializer.Serialize(flightDetails);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpoint, stringContent);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    NotFound();
                }
            }
        }
        [HttpGet("Additional Information")]
        public async Task<string> Information([Required][FromQuery(Name = "Country")] string country)
        {
            string chosenCountry = $"\"{country}\"";
            string url = "https://localhost:7163";
            string endpoint = "/Information";
            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                StringContent stringContent = new StringContent(chosenCountry, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpoint, stringContent);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch(Exception ex)
                {
                    return "Not Found";
                }
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
    }
}

