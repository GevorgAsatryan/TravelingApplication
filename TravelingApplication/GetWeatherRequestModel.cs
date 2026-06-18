using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace TravelingApplication
{
    public class GetWeatherRequestModel
    {
        [FromQuery(Name = "Where would you like to travel (City)")]
        public string City { get; set; }
        [FromQuery(Name = "Country")]
        public string Country { get; set; }
    }

    public class GetWeatherRequestModelValidator : AbstractValidator<GetWeatherRequestModel>
    {
        public GetWeatherRequestModelValidator()
        {
            RuleFor(model => model.City).NotEmpty().WithMessage("City is required, Fluent Validation is working");
            RuleFor(model => model.Country).NotEmpty().WithMessage("Country is required");
        }
    }
}