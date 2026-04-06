using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BizI.Application.Features.CustomerGroups;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BizI.Application.Tests.Features.CustomerGroups;

public class CustomerGroupHandlerTests
{
    private readonly Mock<IRepository<CustomerGroup>> _mockRepo;
    private readonly Mock<ILogger<CreateCustomerGroupHandler>> _mockCreateLogger;
    private readonly Mock<ILogger<UpdateCustomerGroupHandler>> _mockUpdateLogger;
    private readonly Mock<ILogger<DeleteCustomerGroupHandler>> _mockDeleteLogger;

    public CustomerGroupHandlerTests()
    {
        _mockRepo = new Mock<IRepository<CustomerGroup>>();
        _mockCreateLogger = new Mock<ILogger<CreateCustomerGroupHandler>>();
        _mockUpdateLogger = new Mock<ILogger<UpdateCustomerGroupHandler>>();
        _mockDeleteLogger = new Mock<ILogger<DeleteCustomerGroupHandler>>();
    }

    [Fact]
    public async Task CreateCustomerGroup_Success()
    {
        var handler = new CreateCustomerGroupHandler(_mockRepo.Object, _mockCreateLogger.Object);
        var command = new CreateCustomerGroupCommand("VIP", 10m);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
        _mockRepo.Verify(x => x.AddAsync(It.IsAny<CustomerGroup>()), Times.Once);
    }

    [Fact]
    public async Task GetAllCustomerGroups_ReturnsData()
    {
        var handler = new GetAllCustomerGroupsHandler(_mockRepo.Object);
        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<CustomerGroup> { new CustomerGroup(), new CustomerGroup() });

        var result = await handler.Handle(new GetAllCustomerGroupsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCustomerGroupById_NotFound_ReturnsNull()
    {
        var handler = new GetCustomerGroupByIdHandler(_mockRepo.Object);
        _mockRepo.Setup(x => x.GetByIdAsync("1")).ReturnsAsync((CustomerGroup?)null);

        var result = await handler.Handle(new GetCustomerGroupByIdQuery("1"), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCustomerGroup_Success()
    {
        var handler = new UpdateCustomerGroupHandler(_mockRepo.Object, _mockUpdateLogger.Object);
        var group = new CustomerGroup { Id = "1", Name = "Old", DiscountPercent = 0 };
        _mockRepo.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(group);

        var result = await handler.Handle(new UpdateCustomerGroupCommand("1", "New", 5m), CancellationToken.None);

        result.Success.Should().BeTrue();
        group.Name.Should().Be("New");
        _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<CustomerGroup>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomerGroup_Success()
    {
        var handler = new DeleteCustomerGroupHandler(_mockRepo.Object, _mockDeleteLogger.Object);

        var result = await handler.Handle(new DeleteCustomerGroupCommand("1"), CancellationToken.None);

        result.Success.Should().BeTrue();
        _mockRepo.Verify(x => x.DeleteAsync("1"), Times.Once);
    }
}
