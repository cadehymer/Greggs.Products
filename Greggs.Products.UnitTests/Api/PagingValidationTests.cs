using FluentAssertions;
using Greggs.Products.Api.Models;
using Greggs.Products.UnitTests.Helpers;
using System.Linq;
using Xunit;

namespace Greggs.Products.UnitTests.Api;

public class PagingValidationTests
{
    private const int PageStart_Default = 0;
    private const int PageStart_Min = 0;
    private const int PageStart_Max = int.MaxValue;

    private const int PageSize_Default = 5;
    private const int PageSize_Min = 0;
    private const int PageSize_Max = 100;

    private const string RangeMessageFormat = "Must be between {0} and {1}";

    [Fact]
    public void HasCorectDefaultValues()
    {
        // Arrange
        ListQuery request = new();

        // Assert
        request.PageStart.Should().Be(PageStart_Default);
        request.PageSize.Should().Be(PageSize_Default);
    }

    [Theory]
    [InlineData(PageStart_Min)]
    [InlineData(PageStart_Min + 1)]
    [InlineData(PageStart_Max - 1)]
    [InlineData(PageStart_Max)]
    public void PageStart_ValidationSuccess(int value)
    {
        // Arrange
        ListQuery request = new() { PageStart = value };

        // Act
        var results = ValidationHelper.ValidateModel(request);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void PageStart_ValidationError_LessThanMin()
    {
        // Arrange
        ListQuery request = new() { PageStart = -1 };

        // Act
        var results = ValidationHelper.ValidateModel(request);

        // Assert
        results.Should().ContainSingle();
        results[0].ErrorMessage.Should().Be(string.Format(RangeMessageFormat, PageStart_Min, PageStart_Max));
        results[0].MemberNames.Should().ContainSingle();
        results[0].MemberNames.Single().Should().Be(nameof(ListQuery.PageStart));
    }

    [Theory]
    [InlineData(PageSize_Min)]
    [InlineData(PageSize_Min + 1)]
    [InlineData(PageSize_Max - 1)]
    [InlineData(PageSize_Max)]
    public void PageSize_ValidationSuccess(int value)
    {
        // Arrange
        ListQuery request = new() { PageSize = value };

        // Act
        var results = ValidationHelper.ValidateModel(request);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public void PageSize_ValidationError_LessThanMin()
    {
        // Arrange
        ListQuery request = new() { PageSize = -1 };

        // Act
        var results = ValidationHelper.ValidateModel(request);

        // Assert
        results.Should().ContainSingle();
        results[0].ErrorMessage.Should().Be(string.Format(RangeMessageFormat, PageSize_Min, PageSize_Max));
        results[0].MemberNames.Should().ContainSingle();
        results[0].MemberNames.Single().Should().Be(nameof(ListQuery.PageSize));
    }

    [Fact]
    public void PageSize_ValidationError_GreaterThanMax()
    {
        // Arrange
        ListQuery request = new() { PageSize = 101 };

        // Act
        var results = ValidationHelper.ValidateModel(request);

        // Assert
        results.Should().ContainSingle();
        results[0].ErrorMessage.Should().Be(string.Format(RangeMessageFormat, PageSize_Min, PageSize_Max));
        results[0].MemberNames.Should().ContainSingle();
        results[0].MemberNames.Single().Should().Be(nameof(ListQuery.PageSize));
    }
}