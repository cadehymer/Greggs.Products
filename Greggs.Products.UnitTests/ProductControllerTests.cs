using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Frameworks.CurrencyConversion.Abstractions;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductControllerTests
{
    private readonly Mock<IDataAccess<Api.DataAccess.Product>> _products = new();
    private readonly Mock<ICurrencyConverterFactory> _currencyConverterFactory = new();
    private readonly Mock<ILogger<ProductController>> _logger = new();
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _controller = new(
            _products.Object,
            _currencyConverterFactory.Object,
            _logger.Object);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(10, 50)]
    [InlineData(500, 100)]
    public void Paging_Values(int pageStart, int pageSize)
    {
        // Arrange
        _products.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()));
        ProductListQuery query = new()
        {
            PageStart = pageStart,
            PageSize = pageSize,
            CurrencyCode = "GBP",
        };

        // Act
        _controller.Get(query);

        // Assert
        _products.Verify(x => x.List(pageStart, pageSize), Times.Once);
    }

    [Theory]
    [InlineData("GBP")]
    [InlineData("EUR")]
    [InlineData("USD")]
    public void Currency_Values(string currencyCode)
    {
        // Arrange
        ProductListQuery query = new()
        {
            CurrencyCode = currencyCode,
        };
        List<Api.DataAccess.Product> products = new()
        {
            new() { Name = "Alpha", PriceInPounds = 1.1m },
            new() { Name = "Beta", PriceInPounds = 2.2m },
            new() { Name = "Gamma", PriceInPounds = 3.3m },
        };
        Mock<ICurrencyConverter> converter = new();
        converter.Setup(x => x.CurrencyCode).Returns(currencyCode);
        converter.Setup(x => x.Convert(It.IsAny<decimal>())).Returns(1);
        _products.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);
        _currencyConverterFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(converter.Object);

        // Act
        var results = _controller.Get(query).ToList();

        // Assert
        _currencyConverterFactory.Verify(x => x.Create(It.IsAny<string>()), Times.Once);
        _currencyConverterFactory.Verify(x => x.Create(currencyCode), Times.Once);
        converter.Verify(x => x.Convert(It.IsAny<decimal>()), Times.Exactly(products.Count));
        foreach (var product in products)
        {
            converter.Verify(x => x.Convert(product.PriceInPounds), Times.Once);
        }
    }
}