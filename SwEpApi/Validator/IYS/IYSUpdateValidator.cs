using FluentValidation;
using SwEpApi.Services.Tenants.Base.IYS.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Validator.IYS
{
    public class IYSUpdateValidator : AbstractValidator<IYSUpdateRequestParams>
    {
        public IYSUpdateValidator()
        {
            RuleFor(x => x.status)
                 .NotEmpty().WithMessage("Status is required.");

            RuleFor(x => x.recipient)
                 .NotEmpty().WithMessage("Recipient is required.");

            RuleFor(x => x.recipientType)
                .NotEmpty().WithMessage("RecipientType is required.");

            RuleFor(x => x.type)
                .NotEmpty().WithMessage("Type is required.");

            RuleFor(x => x.creationDate)
                .NotEmpty().WithMessage("CreationDate is required.");
        }
    }
}
