using FluentValidation;

namespace TravelingApplication
{
    public class GetWeatherRequestModelValidator : AbstractValidator<GetWeatherRequestModel>
    {
        public GetWeatherRequestModelValidator()
        {
            RuleFor(model => model.City).NotEmpty().WithMessage("City is required, Fluent Validation is working");
            RuleFor(model => model.Country).NotEmpty().WithMessage("Country is required");
        }
    }
}
