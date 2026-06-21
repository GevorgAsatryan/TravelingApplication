using FluentValidation;

namespace TravelingApplication
{
    public class GetInformationRequestModelValidator : AbstractValidator<GetInformationRequestModel>
    {
        public GetInformationRequestModelValidator()
        {
            RuleFor(model => model.Country).NotEmpty().WithMessage("Country is required.");
        }
    }
}
