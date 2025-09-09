using Greggs.Products.CurrencyConversion.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Greggs.Products.CurrencyConversion.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class CurrencyCodeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var options = validationContext.GetRequiredService<IOptions<CurrencyConverterOptions>>().Value;

        if (value is not string code ||
            !code.Equals(options.DefaultCurrency, StringComparison.InvariantCultureIgnoreCase) &&
            (options.ExchangeRates == null ||
            !options.ExchangeRates.ContainsKey(code.ToUpper())))
        {
            return new(ErrorMessage ?? $"{validationContext.DisplayName} is not valid",
                new string[] { validationContext.MemberName! });
        }

        return ValidationResult.Success;
    }
}