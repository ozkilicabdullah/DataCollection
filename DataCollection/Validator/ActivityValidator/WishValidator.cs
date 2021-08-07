using DataCollection.Contracts;
using FluentValidation;


namespace DataCollection.Validator.ActivityValidator
{
    public class WishValidator : AbstractValidator<WishParams>
    {
        public WishValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.");
            RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("ProductID is required.");
        }
    }
}
