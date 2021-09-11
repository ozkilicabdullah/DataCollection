using DataCollection.Contracts;
using FluentValidation;


namespace DataCollection.Validator.ActivityValidator
{
    public class BasketValidator : AbstractValidator<BasketParams>
    {
        public BasketValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is requeired.");
            RuleFor(x => x.ProductID)
               .NotEmpty().WithMessage("ProductID is requeired.");

        }
    }
}
