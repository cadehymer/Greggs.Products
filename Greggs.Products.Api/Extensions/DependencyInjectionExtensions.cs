using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.Api.Extensions;

internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddProducts(this IServiceCollection services)
    {
        services.AddSingleton<IDataAccess<Product>, ProductAccess>();

        return services;
    }
}