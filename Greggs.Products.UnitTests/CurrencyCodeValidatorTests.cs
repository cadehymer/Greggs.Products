using FluentAssertions;
using Greggs.Products.Api.Frameworks.CurrencyConversion;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Greggs.Products.UnitTests;

public class CurrencyCodeValidatorTests
{
    private readonly Mock<IServiceProvider> _serviceProvider = new();
    private readonly CurrencyConverterOptions _options = new();

    public CurrencyCodeValidatorTests()
    {
        _options.DefaultCurrency = null;
        _options.ExchangeRates = null;
        _serviceProvider.Setup(x => x.GetService(typeof(IOptions<CurrencyConverterOptions>))).Returns(Options.Create(_options));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Error_Empty(string value)
    {
        // Arrange
        ProductListQuery model = new() { CurrencyCode = value };
        ValidationContext context = new(model, _serviceProvider.Object, null);
        List<ValidationResult> results = new();

        // Act
        bool valid = Validator.TryValidateObject(model, context, results, true);

        // Assert
        valid.Should().BeFalse();
        results.Should().ContainSingle();
        results[0].MemberNames.Should().ContainSingle();
        results[0].MemberNames.Single().Should().Be(nameof(ProductListQuery.CurrencyCode));
    }

    [Fact]
    public void Error_NotFound()
    {
        // Arrange
        ProductListQuery model = new() { CurrencyCode = "ABC" };
        ValidationContext context = new(model, _serviceProvider.Object, null);
        List<ValidationResult> results = new();

        // Act
        bool valid = Validator.TryValidateObject(model, context, results, true);

        // Assert
        valid.Should().BeFalse();
        results.Should().ContainSingle();
        results[0].MemberNames.Should().ContainSingle();
        results[0].MemberNames.Single().Should().Be(nameof(ProductListQuery.CurrencyCode));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("ABC")]
    [InlineData("abc")]
    public void Success_Default(string value)
    {
        _options.DefaultCurrency = "ABC";

        // Arrange
        ProductListQuery model = new() { CurrencyCode = value };
        ValidationContext context = new(model, _serviceProvider.Object, null);
        List<ValidationResult> results = new();

        // Act
        bool valid = Validator.TryValidateObject(model, context, results, true);

        // Assert
        valid.Should().BeTrue();
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("abc")]
    public void Success_Found(string value)
    {
        _options.ExchangeRates = new() { { "ABC", 1.23m } };

        // Arrange
        ProductListQuery model = new() { CurrencyCode = value };
        ValidationContext context = new(model, _serviceProvider.Object, null);
        List<ValidationResult> results = new();

        // Act
        bool valid = Validator.TryValidateObject(model, context, results, true);

        // Assert
        valid.Should().BeTrue();
    }
}