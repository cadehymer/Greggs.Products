using System.Collections.Generic;
namespace Greggs.Products.Api.Frameworks.CurrencyConversion;

public sealed class CurrencyConverterOptions
{
    public const string ConfigurationSection = "Currency";

    public string DefaultCurrency { get; set; } = "GBP";

    public Dictionary<string, decimal> ExchangeRates { get; set; }
}