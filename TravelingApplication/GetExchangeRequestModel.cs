using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelingApplication
{
    public class GetExchangeRequestModel
    {
        [FromQuery(Name = "Amount")]
        public double Amount { get; set; }

        [FromQuery(Name = "Convert from")]
        public string BaseCurrency { get; set; }

        [FromQuery(Name = "To")]
        public string PreferredCurrency { get; set; }

    }
}
