using Greggs.Products.Api.Frameworks.CurrencyConversion;
using Microsoft.AspNetCore.Mvc;

namespace Greggs.Products.Api.Models;

/// <summary>
/// Represents a request for a product list by start index and page size.
/// </summary>
public sealed class ProductListQuery : ListQuery
{
    /// <summary>
    /// The currency code (optional).
    /// </summary>
    [FromQuery(Name = "currencyCode")]
    [CurrencyCode]
    public string CurrencyCode { get; set; }
}