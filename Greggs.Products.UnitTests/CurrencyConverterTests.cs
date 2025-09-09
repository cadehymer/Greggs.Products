using FluentAssertions;
using Greggs.Products.Api.Frameworks.CurrencyConversion;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Xunit;

namespace Greggs.Products.UnitTests;

public class CurrencyConverterTests
{
    private readonly CurrencyConverterOptions _options = new();
    private readonly CurrencyConverter _converter;

    public CurrencyConverterTests()
    {
        _options.DefaultCurrency = null;
        _options.ExchangeRates = null;
        _converter = new(Options.Create(_options));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void GetDefaultCurrencyCode_NoDefaultCurrency_Throw(string value)
    {
        // Arrange
        _options.DefaultCurrency = value;
        Action action = () => _converter.GetDefaultCurrencyCode();

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("Default currency is not set");
    }

    [Theory]
    [InlineData("GBP")]
    [InlineData("EUR")]
    [InlineData("USD")]
    public void GetDefaultCurrencyCode_FromConfiguration(string value)
    {
        // Arrange
        _options.DefaultCurrency = value;

        // Act
        string code = _converter.GetDefaultCurrencyCode();

        // Assert
        code.Should().Be(value);

    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Convert_EmptyCode_Throw(string code)
    {
        // Arrange
        Action action = () => _converter.Convert(1.23m, code);

        // Assert
        action.Should().Throw<ArgumentException>().WithParameterName("currencyCode");
    }

    [Theory]
    [InlineData("GBP")]
    [InlineData("EUR")]
    [InlineData("USD")]
    public void Convert_NotFound_Throws(string code)
    {
        // Arrange
        _options.DefaultCurrency = "ABC";
        Action action = () => _converter.Convert(1, code);

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("Invalid currency code");
    }

    [Theory]
    [InlineData(1.23, "GBP")]
    [InlineData(4.56, "EUR")]
    [InlineData(7.89, "USD")]
    public void Convert_DefaultCurrency_CorrectResult(decimal value, string code)
    {
        // Arrange
        _options.DefaultCurrency = code;

        // Act
        decimal result = _converter.Convert(value, code);

        // Assert
        result.Should().Be(value);
    }

    [Theory]
    [InlineData(1.23, "GBP", 2.2)]
    [InlineData(4.56, "EUR", 3.3)]
    [InlineData(7.89, "USD", 4.4)]
    public void Convert_CorrectResult(decimal value, string code, decimal rate)
    {
        // Arrange
        _options.DefaultCurrency = "ABC";
        _options.ExchangeRates = new Dictionary<string, decimal>
        {
            { code, rate }
        };

        // Act
        decimal result = _converter.Convert(value, code);

        // Assert
        result.Should().Be(Math.Round(value * rate, 2, MidpointRounding.AwayFromZero));
    }
}