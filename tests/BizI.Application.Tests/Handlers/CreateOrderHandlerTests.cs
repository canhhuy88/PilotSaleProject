using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BizI.Application.Features.Orders;
using BizI.Application.Features.Products;
using BizI.Domain.Entities;
using BizI.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace BizI.Application.Tests.Handlers;

public class CreateOrderHandlerTests : BaseTest
{
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _handler = new CreateOrderHandler(OrderRepoMock.Object, InventoryServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateOrder_WhenStockIsAvailable()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();
        var command = new CreateOrderCommand(
            new List<OrderItemDto> { new(productId, 5, 100) },
            warehouseId
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        OrderRepoMock.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
        InventoryServiceMock.Verify(x => x.ExportStockAsync(productId, warehouseId, 5, It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenInventoryServiceThrowsException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();
        var command = new CreateOrderCommand(
            new List<OrderItemDto> { new(productId, 5, 100) },
            warehouseId
        );

        InventoryServiceMock
            .Setup(x => x.ExportStockAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<Guid?>()))
            .ThrowsAsync(new DomainException("Insufficient stock"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Insufficient stock");
    }
}
