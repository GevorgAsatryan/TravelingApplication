using FluentValidation;

namespace TravelingApplication
{
    public class GetUserLoginRequestModelValidator : AbstractValidator<GetUserLoginRequestModel>
    {
        public GetUserLoginRequestModelValidator()
        {
            RuleFor(model => model.Email).NotEmpty();
            RuleFor(model => model.Password).NotEmpty();
        }
    }
}
