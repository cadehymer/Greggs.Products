using FluentAssertions;
using Greggs.Products.CurrencyConversion.Converters;
using System;
using Xunit;

namespace Greggs.Products.UnitTests.CurrencyConversion;

public class CurrencyConverterTests
{
    [Theory]
    [InlineData(1.23, "GBP")]
    [InlineData(4.56, "EUR")]
    [InlineData(7.89, "USD")]
    public void DefaultConverter_CorrectResult(decimal value, string code)
    {
        // Arrange
        DefaultCurrencyConverter converter = new(code);

        // Act
        decimal result = converter.Convert(value);

        // Assert
        result.Should().Be(value);
    }

    [Theory]
    [InlineData(1.23, "GBP", 2.2)]
    [InlineData(4.56, "EUR", 3.3)]
    [InlineData(7.89, "USD", 4.4)]
    public void Converter_CorrectResult(decimal value, string code, decimal rate)
    {
        // Arrange
        CurrencyConverter converter = new(code, rate);
        decimal expectedValue = Math.Round(value * rate, 2, MidpointRounding.AwayFromZero);

        // Act
        decimal result = converter.Convert(value);

        // Assert
        result.Should().Be(expectedValue);
    }
}