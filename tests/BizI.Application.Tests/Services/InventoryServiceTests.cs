using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BizI.Application.Services;
using BizI.Domain.Entities;
using BizI.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Tests.Services;

public class InventoryServiceTests : BaseTest
{
    private readonly InventoryService _service;
    private readonly Mock<ILogger<InventoryService>> _loggerMock;

    public InventoryServiceTests()
    {
        _loggerMock = new Mock<ILogger<InventoryService>>();
        _service = new InventoryService(InventoryRepoMock.Object, TransactionRepoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ImportStock_ShouldIncreaseStockAndCreateTransaction()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();
        var inventory = new Inventory { ProductId = productId, WarehouseId = warehouseId, Quantity = 10 };

        InventoryRepoMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Inventory, bool>>>()))
            .ReturnsAsync(new List<Inventory> { inventory });

        // Act
        await _service.ImportStockAsync(productId, warehouseId, 5);

        // Assert
        inventory.Quantity.Should().Be(15);
        InventoryRepoMock.Verify(x => x.UpdateAsync(inventory), Times.Once);
        TransactionRepoMock.Verify(x => x.AddAsync(It.Is<InventoryTransaction>(t =>
            t.ProductId == productId &&
            t.WarehouseId == warehouseId &&
            t.Quantity == 5 &&
            t.Type == InventoryTransactionType.Import)), Times.Once);
    }

    [Fact]
    public async Task ExportStock_ShouldDecreaseStockAndCreateTransaction()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();
        var inventory = new Inventory { ProductId = productId, WarehouseId = warehouseId, Quantity = 10 };

        InventoryRepoMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Inventory, bool>>>()))
            .ReturnsAsync(new List<Inventory> { inventory });

        // Act
        await _service.ExportStockAsync(productId, warehouseId, 3);

        // Assert
        inventory.Quantity.Should().Be(7);
        InventoryRepoMock.Verify(x => x.UpdateAsync(inventory), Times.Once);
        TransactionRepoMock.Verify(x => x.AddAsync(It.Is<InventoryTransaction>(t =>
            t.ProductId == productId &&
            t.WarehouseId == warehouseId &&
            t.Quantity == 3 &&
            t.Type == InventoryTransactionType.Export)), Times.Once);
    }

    [Fact]
    public async Task ExportStock_ShouldFail_WhenStockIsInsufficient()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();
        var inventory = new Inventory { ProductId = productId, WarehouseId = warehouseId, Quantity = 2 };

        InventoryRepoMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Inventory, bool>>>()))
            .ReturnsAsync(new List<Inventory> { inventory });

        // Act & Assert
        var act = () => _service.ExportStockAsync(productId, warehouseId, 5);
        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task ExportStock_ShouldPreventNegativeStock()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();

        InventoryRepoMock
            .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Inventory, bool>>>()))
            .ReturnsAsync(new List<Inventory>()); // No inventory found

        // Act & Assert
        var act = () => _service.ExportStockAsync(productId, warehouseId, 1);
        await act.Should().ThrowAsync<Exception>();
    }
}
