using FluentAssertions;
using Greggs.Products.CurrencyConversion;
using Greggs.Products.CurrencyConversion.Abstractions;
using Greggs.Products.CurrencyConversion.Configuration;
using Greggs.Products.CurrencyConversion.Converters;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace Greggs.Products.UnitTests.CurrencyConversion;

public class CurrencyConverterFactoryTests
{
    private readonly CurrencyConverterOptions _options = new();
    private readonly CurrencyConverterFactory _factory;

    public CurrencyConverterFactoryTests()
    {
        _options.DefaultCurrency = null;
        _options.ExchangeRates = null;
        _factory = new(Options.Create(_options));
    }

    [Theory]
    [InlineData("GBP")]
    [InlineData("EUR")]
    [InlineData("USD")]
    public void CreateDefaultConverter(string value)
    {
        // Arrange
        _options.DefaultCurrency = value;

        // Act
        ICurrencyConverter converter = _factory.CreateDefault();

        // Assert
        converter.Should().BeOfType<DefaultCurrencyConverter>();
        converter.CurrencyCode.Should().Be(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateDefaultConverter_NoDefaultCurrency_Throw(string value)
    {
        // Arrange
        _options.DefaultCurrency = value;
        Action action = () => _factory.CreateDefault();

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("Default currency is not set");
    }

    [Theory]
    [InlineData("GBP", 1.23)]
    [InlineData("EUR", 2.34)]
    [InlineData("USD", 3.45)]
    public void CreateConverter(string code, decimal rate)
    {
        // Arrange
        _options.ExchangeRates = new() { { code, rate } };

        // Act
        ICurrencyConverter converter = _factory.Create(code);

        // Assert
        converter.Should().BeOfType<CurrencyConverter>();
        converter.CurrencyCode.Should().Be(code);
        converter.ExchangeRate.Should().Be(rate);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateConverter_EmptyCode_Throw(string code)
    {
        // Arrange
        Action action = () => _factory.Create(code);

        // Assert
        action.Should().Throw<ArgumentException>().WithParameterName("currencyCode");
    }

    [Theory]
    [InlineData("GBP")]
    [InlineData("EUR")]
    [InlineData("USD")]
    public void CreateConverter_CodeNotFound_Throw(string code)
    {
        // Arrange
        Action action = () => _factory.Create(code);

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("Invalid currency code");
    }
}