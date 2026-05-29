using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetFlightBookingRequestModel
    {
        [Required]
        [FromQuery(Name = "Flying from")] 
        public string FromCity { get; set; }
        [Required]
        [FromQuery(Name = "Flying to")] 
        public string ToCity { get; set; }
        [Required]
        [FromQuery(Name = "Departing")] 
        public DateTime Departing { get; set; }
        [FromQuery(Name = "Returning (Skip if you want one way)")] 
        public DateTime? Returning { get; set; }
    }
}