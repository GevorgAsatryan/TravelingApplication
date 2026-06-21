using FluentValidation;

namespace TravelingApplication
{
    public class GetFlightBookingRequestModelValidator : AbstractValidator<GetFlightBookingRequestModel>
    {
        public GetFlightBookingRequestModelValidator()
        {
            RuleFor(model => model.FromCity).NotEmpty().WithMessage("FromCity is required");
            RuleFor(model => model.ToCity).NotEmpty().WithMessage("ToCity is required");
            RuleFor(model => model.Departing).NotEmpty().WithMessage("Departing date is required");
        }
    }
}
