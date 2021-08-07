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
            RuleFor(x => x.ProductId)
               .NotEmpty().WithMessage("ProductId is requeired.");
        }
    }
}
