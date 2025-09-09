using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Frameworks.CurrencyConversion;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductControllerTests
{
    private readonly Mock<IDataAccess<Api.DataAccess.Product>> _products = new();
    private readonly Mock<ICurrencyConverter> _currencyConverter = new();
    private readonly Mock<ILogger<ProductController>> _logger = new();
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _controller = new(
            _products.Object,
            _currencyConverter.Object,
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
        var results = _controller.Get(query);

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
        var products = new Api.DataAccess.Product[]
        {
            new() { Name = "Alpha", PriceInPounds = 1.1m },
            new() { Name = "Beta", PriceInPounds = 2.2m },
            new() { Name = "Gamma", PriceInPounds = 3.3m },
        };
        _products.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);
        _currencyConverter.Setup(x => x.Convert(It.IsAny<decimal>(), It.IsAny<string>())).Returns(1);
        ProductListQuery query = new()
        {
            CurrencyCode = currencyCode,
        };

        // Act
        var results = _controller.Get(query).ToList();

        // Assert
        _currencyConverter.Verify(x => x.Convert(It.IsAny<decimal>(), It.IsAny<string>()), Times.Exactly(products.Length));
        _currencyConverter.Verify(x => x.Convert(1.1m, currencyCode), Times.Once);
        _currencyConverter.Verify(x => x.Convert(2.2m, currencyCode), Times.Once);
        _currencyConverter.Verify(x => x.Convert(3.3m, currencyCode), Times.Once);
    }
}