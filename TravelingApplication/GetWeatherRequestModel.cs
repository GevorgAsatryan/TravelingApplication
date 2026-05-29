using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetWeatherRequestModel
    {

        [Required]
        [FromQuery(Name = "Where would you like to travel (City)")]
        public string City { get; set; }
        [Required]
        [FromQuery(Name = "Country")]
        public string Country { get; set; }
    }
}