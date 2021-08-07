using FluentValidation;
using FluentValidation.Results;
using DataCollection.Model.Request;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataCollection.Validator
{

    public class ActionRequestModelValidator : AbstractValidator<ActionRequestModel>
    {

        public ActionRequestModelValidator()
        {
            RuleForEach(x => x.Action).SetValidator(new ActionValidator());
        }

    }
        
    public class ActionValidator : AbstractValidator<ActionRequest>
    {

        public ActionValidator()
        {
            RuleFor(x => x.Action).NotEmpty().WithMessage("Action boş olamaz");
            RuleFor(x => x.Payload).NotEmpty().WithMessage("Payload parametrelerini belirtiniz");
        }

        protected override bool PreValidate(ValidationContext<ActionRequest> context, FluentValidation.Results.ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure("", "Parametreler sağlanmamış."));
                return false;
            }

            return true;
        }       
    }
}
