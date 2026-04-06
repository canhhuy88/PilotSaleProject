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
        var product = Product.Create("T", "T", 0, 0, "T", Guid.NewGuid());
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Order_ShouldInitializeItemsList()
    {
        var order = Order.Create("O", Guid.NewGuid(), "T", new List<OrderItem>{ OrderItem.Create(Guid.NewGuid(), 1, 10) });
        order.Items.Should().NotBeNull();
    }
}
