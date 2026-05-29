using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetHotelBookingRequestModel
    {
        [Required]
        [FromQuery(Name = "Country or city")] 
        public string CountryOrCity { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Checkin { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Checkout { get; set; }
        [Required]
        [Range(0, 30, ErrorMessage = "Adults must be between 0 and 30.")]
        [FromQuery(Name = "Number of Adults")]
        public int Adults { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "Children must be between 0 and 10.")]
        [FromQuery(Name = "Number of children")]
        public int Children { get; set; }
    }
}