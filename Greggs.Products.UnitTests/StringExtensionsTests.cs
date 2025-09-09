using FluentAssertions;
using Greggs.Products.Api.Extensions;
using Xunit;

namespace Greggs.Products.UnitTests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData(null, "foobar", "foobar")]
    [InlineData("", "foobar", "foobar")]
    [InlineData("foo", "bar", "foo")]
    public void IfNullOrEmpty_ReturnsDefaultValue(string value, string defaultValue, string expectedResult)
    {
        // Act
        string result = value.IfNullOrEmpty(defaultValue);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, "foobar", "foobar")]
    [InlineData("", "foobar", "foobar")]
    [InlineData("foo", "bar", "foo")]
    public void IfNullOrEmpty_ReturnsDefaultValue_Func(string value, string defaultValue, string expectedResult)
    {
        // Act
        string result = value.IfNullOrEmpty(() => defaultValue);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IfNullOrEmpty_ReturnsNull(string value)
    {
        // Arrange
        string defaultValue = null;

        // Act
        string result = value.IfNullOrEmpty(defaultValue);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IfNullOrEmpty_ReturnsNull_Func(string value)
    {
        // Act
        string result = value.IfNullOrEmpty(() => null);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IfNullOrEmpty_ReturnsEmpty(string value)
    {
        // Act
        string result = value.IfNullOrEmpty("");

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IfNullOrEmpty_ReturnsEmpty_Func(string value)
    {
        // Act
        string result = value.IfNullOrEmpty(() => "");

        // Assert
        result.Should().BeEmpty();
    }
}