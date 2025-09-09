using System.Collections.Generic;

namespace Greggs.Products.Api.Models;

public class Product
{
    public string Name { get; set; }
    public Dictionary<string, decimal> Price { get; set; }
}