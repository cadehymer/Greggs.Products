using Greggs.Products.CurrencyConversion.Abstractions;
using Greggs.Products.CurrencyConversion.Configuration;
using Greggs.Products.CurrencyConversion.Converters;
using Microsoft.Extensions.Options;

namespace Greggs.Products.CurrencyConversion;

public class CurrencyConverterFactory : ICurrencyConverterFactory
{
    private readonly CurrencyConverterOptions _options;

    public CurrencyConverterFactory(IOptions<CurrencyConverterOptions> options)
    {
        _options = options.Value;
    }

    public ICurrencyConverter CreateDefault()
    {
        if (string.IsNullOrWhiteSpace(_options.DefaultCurrency))
        {
            throw new InvalidOperationException("Default currency is not set");
        }

        return new DefaultCurrencyConverter(_options.DefaultCurrency);
    }

    public ICurrencyConverter Create(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            throw new ArgumentException("Parameter was not set correctly", nameof(currencyCode));
        }

        if (currencyCode == _options.DefaultCurrency)
        {
            return new DefaultCurrencyConverter(currencyCode);
        }

        if (_options.ExchangeRates != null &&
            _options.ExchangeRates.TryGetValue(currencyCode, out decimal rate))
        {
            return new CurrencyConverter(currencyCode, rate);
        }

        throw new InvalidOperationException("Invalid currency code");
    }
}