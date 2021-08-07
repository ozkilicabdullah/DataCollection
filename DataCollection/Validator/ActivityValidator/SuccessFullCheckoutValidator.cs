using DataCollection.Contracts;
using FluentValidation;

namespace DataCollection.Validator.ActivityValidator
{
    public class SuccessFullCheckoutValidator : AbstractValidator<SuccessFullCheckoutParams>
    {
        public SuccessFullCheckoutValidator()
        {
            RuleFor(x => x.DeliveryAddressId)
                .NotEmpty().WithMessage("DeliveryAddressId is required.");
            RuleFor(x => x.DeliveryType)
                .NotEmpty().WithMessage("DeliveryType is required.");
            RuleFor(x => x.OrderedItems)
                .NotEmpty().WithMessage("OrderedItems is required.");
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.");
            RuleFor(x => x.PaymentTypeId)
                .NotEmpty().WithMessage("PaymentType is required.");

        }
    }
}
