using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetInformationRequestModel
    {
        [FromQuery(Name = "Country")]
        public string Country { get; set; }
    }
}