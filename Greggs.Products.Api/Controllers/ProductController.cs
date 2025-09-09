using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Frameworks.CurrencyConversion.Abstractions;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Product = Greggs.Products.Api.Models.Product;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IDataAccess<DataAccess.Product> _products;
    private readonly ICurrencyConverterFactory _currencyConverterFactory;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IDataAccess<DataAccess.Product> products,
        ICurrencyConverterFactory currencyConverterFactory,
        ILogger<ProductController> logger)
    {
        _products = products;
        _currencyConverterFactory = currencyConverterFactory;
        _logger = logger;
    }

    /// <summary>
    /// Gets a list of products
    /// </summary>
    /// <param name="query">The query parameters</param>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<Product> Get([FromQuery] ProductListQuery query)
    {
        _logger.LogDebug("Requested products: PageStart: {PageStart}, PageSize: {PageSize}, Currency: {Currency}",
            query.PageStart, query.PageSize, query.CurrencyCode);

        IEnumerable<DataAccess.Product> products = _products.List(query.PageStart, query.PageSize);

        ICurrencyConverter converter = string.IsNullOrEmpty(query.CurrencyCode)
            ? _currencyConverterFactory.CreateDefault()
            : _currencyConverterFactory.Create(query.CurrencyCode.ToUpper());

        return products
            .Select(x => new Product
            {
                Name = x.Name,
                Price = new Dictionary<string, decimal>
                {
                    { converter.CurrencyCode, converter.Convert(x.PriceInPounds) },
                },
            });
    }
}