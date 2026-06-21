using FluentValidation;

namespace TravelingApplication
{
    public class GetHotelBookingRequestModelValidator : AbstractValidator<GetHotelBookingRequestModel>
    {
        public GetHotelBookingRequestModelValidator()
        {
            RuleFor(model => model.CountryOrCity).NotEmpty().WithMessage("Country or City is required.");
            RuleFor(model => model.Checkin).NotEmpty().WithMessage("Checkin date is required");
            RuleFor(model => model.Checkout).NotEmpty().WithMessage("Checkout date is required");
            RuleFor(model => model.Adults).NotEmpty().WithMessage("The number of adults is required.");
            RuleFor(model => model.Adults).GreaterThan(0).LessThanOrEqualTo(10).WithMessage("The number of adults must be between 0 and 10.");
            RuleFor(model => model.Children).NotEmpty().WithMessage("The number of children is required.");
            RuleFor(model => model.Children).GreaterThan(0).LessThanOrEqualTo(10).WithMessage("The number of children must be between 0 and 10.");
        }
    }
}
