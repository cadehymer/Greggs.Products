namespace Greggs.Products.Api.Frameworks.CurrencyConversion.Abstractions;

public interface ICurrencyConverter
{
    string CurrencyCode { get; }

    decimal ExchangeRate { get; }

    decimal Convert(decimal value);
}