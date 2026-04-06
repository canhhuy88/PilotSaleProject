using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BizI.Application.Features.ImportOrders;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BizI.Application.Tests.Features.ImportOrders;

public class ImportOrderHandlerTests
{
    private readonly Mock<IRepository<ImportOrder>> _mockRepo = new();
    private readonly Mock<ILogger<CreateImportOrderHandler>> _loggerCreate = new();

    [Fact]
    public async Task GetAll_ReturnsData()
    {
        var handler = new GetAllImportOrdersHandler(_mockRepo.Object);
        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<ImportOrder> { new ImportOrder() });

        var result = await handler.Handle(new GetAllImportOrdersQuery(), CancellationToken.None);

        result.Should().HaveCount(1);
    }
}
