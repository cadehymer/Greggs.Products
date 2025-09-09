using Greggs.Products.CurrencyConversion.Abstractions;
using Greggs.Products.CurrencyConversion.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.CurrencyConversion.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCurrencyConversion(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CurrencyConverterOptions>(configuration.GetSection(CurrencyConverterOptions.ConfigurationSection));
        services.AddSingleton<ICurrencyConverterFactory, CurrencyConverterFactory>();

        return services;
    }
}