using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetExchangeRequestModel
    {
        [Required]
        [FromQuery(Name = "Amount")]
        public double Amount { get; set; }

        [Required]
        [FromQuery(Name = "Convert from")]
        public string BaseCurrency { get; set; }

        [Required]
        [FromQuery(Name = "To")]
        public string PreferredCurrency { get; set; }

    }
}
