using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductControllerTests
{
    private readonly Mock<IDataAccess<Product>> _products = new();
    private readonly Mock<ILogger<ProductController>> _logger = new();
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _controller = new(
            _products.Object,
            _logger.Object);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(10, 50)]
    [InlineData(500, 100)]
    public void PageStart_ValidationSuccess(int pageStart, int pageSize)
    {
        _products.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()));

        // Arrange
        ListQuery query = new()
        {
            PageStart = pageStart,
            PageSize = pageSize,
        };

        // Act
        var results = _controller.Get(query);

        // Assert
        _products.Verify(x => x.List(pageStart, pageSize), Times.Once);
    }
}