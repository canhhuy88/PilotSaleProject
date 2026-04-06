using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BizI.Application.Features.InventoryTransactions;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace BizI.Application.Tests.Features.InventoryTransactions;

public class InventoryTransactionHandlerTests
{
    private readonly Mock<IRepository<InventoryTransaction>> _mockRepo = new();

    [Fact]
    public async Task GetAll_ReturnsData()
    {
        var handler = new GetAllInventoryTransactionsHandler(_mockRepo.Object);
        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<InventoryTransaction> { new InventoryTransaction() });

        var result = await handler.Handle(new GetAllInventoryTransactionsQuery(), CancellationToken.None);

        result.Should().HaveCount(1);
    }
}
