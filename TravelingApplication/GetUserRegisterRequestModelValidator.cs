using FluentValidation;

namespace TravelingApplication
{
    public class GetUserRegisterRequestModelValidator : AbstractValidator<GetUserRegisterRequestModel>
    {
        public GetUserRegisterRequestModelValidator()
        {
            RuleFor(model => model.Email).NotEmpty();
            RuleFor(model => model.Username).NotEmpty();
            RuleFor(model => model.Password).NotEmpty();
        }
    }
}
