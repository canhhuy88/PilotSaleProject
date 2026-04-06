using System;
using BizI.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace BizI.Domain.Tests;

public class EntityTests
{
    [Fact]
    public void Product_ShouldHaveDefaultCreatedAt()
    {
        var product = new Product();
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Order_ShouldInitializeItemsList()
    {
        var order = new Order();
        order.Items.Should().NotBeNull();
    }
}
