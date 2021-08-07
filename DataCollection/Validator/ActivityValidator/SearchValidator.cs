using DataCollection.Contracts;
using FluentValidation;

namespace DataCollection.Validator.ActivityValidator
{
    public class SearchValidator : AbstractValidator<SearchParams>
    {
        public SearchValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value is required.");
        }
    }
}
