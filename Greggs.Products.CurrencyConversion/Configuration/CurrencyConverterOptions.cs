namespace Greggs.Products.CurrencyConversion.Configuration;

public sealed class CurrencyConverterOptions
{
    public const string ConfigurationSection = "Currency";

    public string? DefaultCurrency { get; set; } = "GBP";

    public Dictionary<string, decimal>? ExchangeRates { get; set; }
}