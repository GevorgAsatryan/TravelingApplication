using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlightBookingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        [HttpPost]
        public async Task Book([FromBody] FlightDetails flightDetails)
        {
            string url = "";

            if (flightDetails.Returning is null)
            {
                url = $"https://www.expedia.com/Flights-Search?leg1=from%3A{flightDetails.FromCity}%2Cto%3A{flightDetails.ToCity}%2Cdeparture%3A{flightDetails.Departing}TANYT%2CfromType%3AU%2CtoType%3AU&mode=search&options=carrier%3A%2Ccabinclass%3Aeconomy%2Cmaxhops%3A1%2Cnopenalty%3AN&pageId=0&trip=oneway\r\n";
            }
            else
            {
                url = $"https://www.expedia.com/Flights-Search?leg1=from:{flightDetails.FromCity},to:{flightDetails.ToCity},departure:{flightDetails.Departing}TANYT,fromType:U,toType:U&leg2=from:{flightDetails.ToCity},to:{flightDetails.FromCity},departure:{flightDetails.Returning}TANYT,fromType:U,toType:U&mode=search&options=carrier:,cabinclass:economy,maxhops:1,nopenalty:N&pageId=0&passengers=adults:0,children:0,infantinlap:0&trip=Roundtrip\r\n";
            }
            
            

            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
