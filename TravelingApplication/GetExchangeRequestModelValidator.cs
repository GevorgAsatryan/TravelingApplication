using FluentValidation;

namespace TravelingApplication
{
    public class GetExchangeRequestModelValidator : AbstractValidator<GetExchangeRequestModel>
    {
        public GetExchangeRequestModelValidator()
        {
            RuleFor(model => model.Amount).NotEmpty().WithMessage("Amount is required.");
            RuleFor(model => model.Amount).GreaterThan(0).WithMessage("Amount must be positive.");
            RuleFor(model => model.BaseCurrency).NotEmpty().WithMessage("Base Currency is required.");
            RuleFor(model => model.PreferredCurrency).NotEmpty().WithMessage("Preferred Currency is required.");
        }
    }
}
