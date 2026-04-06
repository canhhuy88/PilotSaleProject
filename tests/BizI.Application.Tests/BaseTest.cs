using Moq;
using BizI.Domain.Interfaces;
using BizI.Domain.Entities;
using BizI.Application.Interfaces;

namespace BizI.Application.Tests;

public abstract class BaseTest
{
    protected readonly Mock<IRepository<Product>> ProductRepoMock = new();
    protected readonly Mock<IRepository<Order>> OrderRepoMock = new();
    protected readonly Mock<IRepository<Warehouse>> WarehouseRepoMock = new();
    protected readonly Mock<IRepository<Inventory>> InventoryRepoMock = new();
    protected readonly Mock<IRepository<InventoryTransaction>> TransactionRepoMock = new();
    protected readonly Mock<IInventoryService> InventoryServiceMock = new();
}
