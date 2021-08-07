using DataCollection.Contracts;
using FluentValidation;

namespace DataCollection.Validator.ActivityValidator
{
    public class SuccessFullCheckoutValidator : AbstractValidator<SuccessFullCheckoutParams>
    {
        public SuccessFullCheckoutValidator()
        {
            RuleFor(x => x.DeliveryAddressID)
                .NotEmpty().WithMessage("DeliveryAddressID is required.");
            RuleFor(x => x.DeliveryType)
                .NotEmpty().WithMessage("DeliveryType is required.");
            RuleFor(x => x.OrderedItems)
                .NotEmpty().WithMessage("OrderedItems is required.");
            RuleFor(x => x.OrderID)
                .NotEmpty().WithMessage("OrderID is required.");
            RuleFor(x => x.PaymentTypeID)
                .NotEmpty().WithMessage("PaymentType is required.");

        }
    }
}
