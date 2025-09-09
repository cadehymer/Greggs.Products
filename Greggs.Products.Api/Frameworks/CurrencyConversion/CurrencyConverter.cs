using Microsoft.Extensions.Options;
using System;

namespace Greggs.Products.Api.Frameworks.CurrencyConversion;

public class CurrencyConverter : ICurrencyConverter
{
    private readonly CurrencyConverterOptions _options;

    public CurrencyConverter(IOptions<CurrencyConverterOptions> options)
    {
        _options = options.Value;
    }

    public string GetDefaultCurrencyCode()
    {
        if (string.IsNullOrWhiteSpace(_options.DefaultCurrency))
        {
            throw new InvalidOperationException("Default currency is not set");
        }

        return _options.DefaultCurrency;
    }

    public decimal Convert(decimal value, string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            throw new ArgumentException("Parameter was not set correctly", nameof(currencyCode));
        }

        if (currencyCode == _options.DefaultCurrency)
        {
            return value;
        }

        if (_options.ExchangeRates != null &&
            _options.ExchangeRates.TryGetValue(currencyCode, out decimal rate))
        {
            return Math.Round(value * rate, 2, MidpointRounding.AwayFromZero);
        }

        throw new InvalidOperationException("Invalid currency code");
    }
}