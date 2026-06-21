using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetHotelBookingRequestModel
    {
        [FromQuery(Name = "Country or city")] 
        public string CountryOrCity { get; set; }

        [DataType(DataType.Date)]
        public DateTime Checkin { get; set; }

        [DataType(DataType.Date)]
        public DateTime Checkout { get; set; }
        
        [FromQuery(Name = "Number of Adults")]
        public int Adults { get; set; }
        
        [FromQuery(Name = "Number of children")]
        public int Children { get; set; }
    }
}