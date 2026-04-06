using System.Threading;
using System.Threading.Tasks;
using BizI.Application.Features.StockOperations;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace BizI.Application.Tests.Features.StockOperations;

public class StockOperationsHandlerTests
{
    private readonly Mock<IRepository<StockIn>> _mockStockInRepo = new();

    [Fact]
    public async Task CreateStockIn_Success()
    {
        var handler = new CreateStockInHandler(_mockStockInRepo.Object);
        var command = new CreateStockInCommand("Code", "S1", "W1", 100, null);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
        _mockStockInRepo.Verify(x => x.AddAsync(It.IsAny<StockIn>()), Times.Once);
    }
}
