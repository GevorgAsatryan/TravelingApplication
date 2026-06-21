using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetWeatherRequestModel
    {
        [FromQuery(Name = "Where would you like to travel (City)")]
        public string City { get; set; }

        [FromQuery(Name = "Country")]
        public string Country { get; set; }
    }
}