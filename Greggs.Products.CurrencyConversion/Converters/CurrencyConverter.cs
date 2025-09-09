using Greggs.Products.CurrencyConversion.Abstractions;

namespace Greggs.Products.CurrencyConversion.Converters;

public class CurrencyConverter : ICurrencyConverter
{
    private readonly string _currencyCode;
    private readonly decimal _exchangeRate;

    public CurrencyConverter(string currencyCode, decimal exchangeRate)
    {
        _currencyCode = currencyCode;
        _exchangeRate = exchangeRate;
    }

    public string CurrencyCode => _currencyCode;

    public decimal ExchangeRate => _exchangeRate;

    public virtual decimal Convert(decimal value) =>
        Math.Round(value * _exchangeRate, 2, MidpointRounding.AwayFromZero);
}