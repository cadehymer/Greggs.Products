using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Extensions;
using Greggs.Products.Api.Frameworks.CurrencyConversion;
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
    private readonly ICurrencyConverter _currencyConverter;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IDataAccess<DataAccess.Product> products,
        ICurrencyConverter currencyConverter,
        ILogger<ProductController> logger)
    {
        _products = products;
        _currencyConverter = currencyConverter;
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

        string currencyCode = query.CurrencyCode.IfNullOrEmpty(() => _currencyConverter.GetDefaultCurrencyCode()).ToUpper();

        return products
            .Select(x => new Product
            {
                Name = x.Name,
                Price = new Dictionary<string, decimal>
                {
                    // TODO: Potentially add support for multiple currencies
                    { currencyCode, _currencyConverter.Convert(x.PriceInPounds, currencyCode) },
                },
            });
    }
}