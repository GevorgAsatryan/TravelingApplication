using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FlightBookingService
{
    public class FlightDetails
    {
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime Departing { get; set; }
        public DateTime? Returning { get; set; }
    }
}
