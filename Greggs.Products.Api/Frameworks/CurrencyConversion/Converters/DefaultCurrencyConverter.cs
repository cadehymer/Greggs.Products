using Greggs.Products.Api.Frameworks.CurrencyConversion.Abstractions;

namespace Greggs.Products.Api.Frameworks.CurrencyConversion.Converters;

public class DefaultCurrencyConverter : CurrencyConverter, ICurrencyConverter
{
    public DefaultCurrencyConverter(string currencyCode)
        : base(currencyCode, 1.0m)
    {
    }

    public override decimal Convert(decimal value) => value;
}