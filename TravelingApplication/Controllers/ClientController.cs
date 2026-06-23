using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly JwtSettings _jwtSettings;
        private readonly AppDbContext _db;

        public ClientController(IHttpClientFactory httpClientFactory, IOptions<JwtSettings> jwtSettings, AppDbContext db)
        {
            _httpClientFactory = httpClientFactory;
            _jwtSettings = jwtSettings.Value;
            _db = db;
        }


        [HttpGet("Login")]
        public async Task<string> Login([Required][FromQuery(Name = "Name")] string name, [Required][EmailAddress(ErrorMessage = "Invalid email address")][FromQuery(Name = "Email")] string email)
        {
            string username = name;
            string userEmail = email;
           
            UserService userService = new UserService(_db);
         
            string secretKey = _jwtSettings.SecretKey;
            string token = TokenManagement.GenerateJwtToken(username, userEmail, secretKey);
            string validation = TokenManagement.ValidateToken(token, secretKey);
            var user = await userService.FindUser(userEmail);
            if (user is null)
            {
                await userService.AddUser(new User() { Email = userEmail, Name = username, Token = token });
            }
            else
            {
                await userService.UpdateUserToken(user, token);
            }

            return "Your token is  " + $"{token}\n\n{validation}";
        }

        [HttpGet("Weather")]
        public async Task<string> Get([FromQuery] GetWeatherRequestModel model)
        {
            string chosenCityAndCountry = $"\"{model.City},{model.Country}\"";

            var httpClient = _httpClientFactory.CreateClient("WeatherClient");

            var responseContent = new StringContent(
                chosenCityAndCountry,
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync("Weather", responseContent);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        [HttpGet("Exchange")]
        public async Task<string> Exchange([FromQuery] GetExchangeRequestModel model)
        {

            var details = new
            {
                amount = model.Amount,
                currency = model.BaseCurrency,
                preferredCurrency = model.PreferredCurrency
            };

            var httpClient = _httpClientFactory.CreateClient("ExchangeClient");

            var json = JsonSerializer.Serialize(details);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Exchange", stringContent);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                NotFound();
            }
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }


        [HttpGet("Book Hotel")]
        [Authorize]
        public async Task<string> BookHotel([FromQuery] GetHotelBookingRequestModel model)
        {
    
            var bookingRequest = new
            {
                countryOrCity = model.CountryOrCity,
                checkin = model.Checkin,
                checkout = model.Checkout,
                adults = model.Adults,
                children = model.Children
            };

            var httpClient = _httpClientFactory.CreateClient("HotelClient");

            string json = JsonSerializer.Serialize(bookingRequest);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Booking", stringContent);
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
        [HttpGet("Book Flight")]
        [Authorize]
        public async Task Flight([FromQuery] GetFlightBookingRequestModel model)
        {
            var flightDetails = new
            {
                fromCity = model.FromCity,
                toCity = model.ToCity,
                departing = model.Departing,
                returning = model.Returning,
            };

            var httpClient = _httpClientFactory.CreateClient("FlightClient");

            var json = JsonSerializer.Serialize(flightDetails);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Booking", stringContent);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                NotFound();
            }
        }
        [HttpGet("Additional Information")]
        public async Task<string> Information([FromQuery] GetInformationRequestModel model)
        {
            string chosenCountry = $"\"{model.Country}\"";

            var httpClient = _httpClientFactory.CreateClient("InformationClient");

            StringContent stringContent = new StringContent(chosenCountry, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Information", stringContent);
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
}

