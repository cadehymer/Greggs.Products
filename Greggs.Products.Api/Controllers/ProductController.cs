using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IDataAccess<Product> _products;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IDataAccess<Product> products,
        ILogger<ProductController> logger)
    {
        _products = products;
        _logger = logger;
    }

    /// <summary>
    /// Gets a list of products
    /// </summary>
    /// <param name="query">The query parameters</param>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<Product> Get([FromQuery] ListQuery query)
    {
        IEnumerable<Product> products = _products.List(query.PageStart, query.PageSize);

        return products;
    }
}