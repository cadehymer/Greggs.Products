namespace Greggs.Products.Api.Frameworks.CurrencyConversion;

public interface ICurrencyConverter
{
    public string GetDefaultCurrencyCode();

    decimal Convert(decimal value, string currencyCode);
}
