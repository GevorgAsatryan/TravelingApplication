using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlightBookingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        [HttpPost]
        public async Task<string> Book([FromBody] FlightDetails flightDetails)
        {
            string url = "";

            if (flightDetails.Returning is not null)
            {
                url = $"https://www.google.com/travel/flights?q=from%3A{flightDetails.FromCity}%20to%3A{flightDetails.ToCity}%20on%20{flightDetails.Departing:yyyy-MM-dd}%20return%20{flightDetails.Returning:yyyy-MM-dd}\r\n";
            }
            else
            {
                url = $"https://www.google.com/travel/flights?q=from:{flightDetails.FromCity} to:{flightDetails.ToCity} on {flightDetails.Departing:yyyy-MM-dd}\r\n";
            }



            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

            return url;
        }
    }
}
