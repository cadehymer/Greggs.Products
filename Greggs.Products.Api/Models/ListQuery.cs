using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Greggs.Products.Api.Models;

/// <summary>
/// Represents a request for a list by start index and page size.
/// </summary>
public sealed class ListQuery
{
    /// <summary>
    /// The zero-based start index.
    /// </summary>
    [FromQuery(Name = "pageStart")]
    [Range(0, int.MaxValue, ErrorMessage = "Must be between {1} and {2}")]
    public int PageStart { get; set; } = 0;

    /// <summary>
    /// The number of items to return.
    /// </summary>
    [FromQuery(Name = "pageSize")]
    [Range(0, 100, ErrorMessage = "Must be between {1} and {2}")]
    public int PageSize { get; set; } = 5;
}