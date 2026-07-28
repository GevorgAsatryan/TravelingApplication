using FluentValidation;

namespace TravelingApplication
{
    public class GetFoodInformationRequestModelValidator : AbstractValidator<GetFoodInformationRequestModel>
    {
        public GetFoodInformationRequestModelValidator()
        {
            RuleFor(model => model.Country).NotEmpty();
        }
    }
}
