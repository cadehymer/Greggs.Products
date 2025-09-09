namespace Greggs.Products.CurrencyConversion.Abstractions;

public interface ICurrencyConverterFactory
{
    ICurrencyConverter Create(string currencyCode);

    ICurrencyConverter CreateDefault();
}