using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BizI.Application.Features.Inventory;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BizI.Application.Tests.Features.Inventory;

public class InventoryHandlerTests
{
    private readonly Mock<IRepository<BizI.Domain.Entities.Inventory>> _mockRepo = new();

    [Fact]
    public async Task GetInventory_All_ReturnsData()
    {
        var handler = new GetInventoryHandler(_mockRepo.Object);
        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<BizI.Domain.Entities.Inventory> { new BizI.Domain.Entities.Inventory() });

        var result = await handler.Handle(new GetInventoryQuery(null, null), CancellationToken.None);

        result.Should().HaveCount(1);
    }
}
