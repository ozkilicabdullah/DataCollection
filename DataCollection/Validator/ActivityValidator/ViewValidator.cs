using DataCollection.Contracts;
using FluentValidation;

namespace DataCollection.Validator.NewFolder.ActivityValidator
{
    public class ViewValidator : AbstractValidator<ViewParams>
    {
        public ViewValidator()
        {
            RuleFor(x => x.Value)
               .NotEmpty().WithMessage("Value is required.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.");
        }
    }
}
