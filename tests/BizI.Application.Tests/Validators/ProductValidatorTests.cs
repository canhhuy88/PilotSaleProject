using BizI.Application.Features.Orders;
using BizI.Application.Features.Products;
using FluentAssertions;
using Xunit;

namespace BizI.Application.Tests.Validators;

public class ProductValidatorTests
{
    private readonly CreateProductCommandValidator _validator = new();

    [Fact]
    public void Validator_ShouldFail_WhenNameIsEmpty()
    {
        var command = new CreateProductCommand("", 100);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_ShouldFail_WhenPriceIsNegative()
    {
        var command = new CreateProductCommand("Test", -1);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_ShouldSucceed_WhenInputIsValid()
    {
        var command = new CreateProductCommand("Test", 100);
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }
}
