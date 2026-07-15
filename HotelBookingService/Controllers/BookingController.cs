using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using System.Text.Json;

namespace HotelBookingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        [HttpPost]
        public async Task<string> Book([FromBody] BookingDetails bookingDetails)
        {
            string url = $"https://www.booking.com/searchresults.html?ss={bookingDetails.CountryOrCity}&checkin_year={bookingDetails.Checkin.Year}&checkin_month={bookingDetails.Checkin.Month}&checkin_monthday={bookingDetails.Checkin.Day}&checkout_year={bookingDetails.Checkout.Year}&checkout_month={bookingDetails.Checkout.Month}&checkout_monthday={bookingDetails.Checkout.Day}&group_adults={bookingDetails.Adults}&group_children={bookingDetails.Children}\r\n";

            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

            return url;
        }
    }
}
