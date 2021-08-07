using DataCollection.Contracts;
using FluentValidation;

namespace DataCollection.Validator.ActivityValidator
{
    public class ReturnedValidator : AbstractValidator<ReturnedParams>
    {
        public ReturnedValidator()
        {
            //RuleFor(x => x.ProductID)
            //    .NotEmpty().WithMessage("ProductID is required");
        }
    }
}
