using FluentValidation;
using SwEpApi.Services.Tenants.Base.Erp;
using SwEpApi.Services.Tenants.Base.Erp.RequestParams;

namespace SwEpApi.Validator.Erp
{
    public class StockListForEpValidator : AbstractValidator<StockListForEpRequestParams>
    {
        public StockListForEpValidator()
        {
            RuleFor(x => x.PriceCurrencyCode)
                .NotEmpty().WithMessage("PriceCurrencyCode is required.");

            RuleFor(x => x.LangCode)
                .NotEmpty().WithMessage("LangCode is required.");

        }

    }
    public class StockPriceListForEpValidator : AbstractValidator<StockPriceListForEpRequestParams>
    {
        public StockPriceListForEpValidator()
        {
            RuleFor(x => x.PriceCurrencyCode)
                .NotEmpty().WithMessage("PriceCurrencyCode is required.");
        }

    }

    public class StockDetailListForEpValidator : AbstractValidator<StockDetailListForEpRequestParams>
    {
        public StockDetailListForEpValidator()
        {
            RuleFor(x => x.PriceCurrencyCode)
                .NotEmpty().WithMessage("PriceCurrencyCode is required.");

            RuleFor(x => x.LangCode)
                .NotEmpty().WithMessage("LangCode is required.");
        }
    }
}
