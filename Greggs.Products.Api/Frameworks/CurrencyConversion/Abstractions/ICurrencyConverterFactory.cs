namespace Greggs.Products.Api.Frameworks.CurrencyConversion.Abstractions;

public interface ICurrencyConverterFactory
{
    ICurrencyConverter Create(string currencyCode);

    ICurrencyConverter CreateDefault();
}