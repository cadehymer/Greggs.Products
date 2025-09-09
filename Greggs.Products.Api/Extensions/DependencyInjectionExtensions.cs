using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Frameworks.CurrencyConversion;
using Greggs.Products.Api.Frameworks.CurrencyConversion.Abstractions;
using Greggs.Products.Api.Frameworks.CurrencyConversion.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.Api.Extensions;

internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddProducts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDataAccess<Product>, ProductAccess>();

        services.Configure<CurrencyConverterOptions>(configuration.GetSection(CurrencyConverterOptions.ConfigurationSection));
        services.AddSingleton<ICurrencyConverterFactory, CurrencyConverterFactory>();

        return services;
    }
}