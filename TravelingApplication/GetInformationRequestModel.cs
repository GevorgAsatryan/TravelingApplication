using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetInformationRequestModel
    {
        [Required]
        [FromQuery(Name = "Country")]
        public string Country { get; set; }
    }
}