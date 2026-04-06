using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BizI.Application.Features.Customers;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BizI.Application.Tests.Features.Customers;

public class CustomerHandlerTests
{
    private readonly Mock<IRepository<Customer>> _mockRepo;
    private readonly Mock<ILogger<CreateCustomerHandler>> _mockCreateLogger;
    private readonly Mock<ILogger<UpdateCustomerHandler>> _mockUpdateLogger;
    private readonly Mock<ILogger<DeleteCustomerHandler>> _mockDeleteLogger;

    public CustomerHandlerTests()
    {
        _mockRepo = new Mock<IRepository<Customer>>();
        _mockCreateLogger = new Mock<ILogger<CreateCustomerHandler>>();
        _mockUpdateLogger = new Mock<ILogger<UpdateCustomerHandler>>();
        _mockDeleteLogger = new Mock<ILogger<DeleteCustomerHandler>>();
    }

    [Fact]
    public async Task CreateCustomer_Success()
    {
        var handler = new CreateCustomerHandler(_mockRepo.Object, _mockCreateLogger.Object);
        var command = new CreateCustomerCommand("Test", "0123456789", "Address", CustomerType.REGULAR, 0);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
        _mockRepo.Verify(x => x.AddAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task GetAllCustomers_ReturnsData()
    {
        var handler = new GetAllCustomersHandler(_mockRepo.Object);
        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Customer> { new Customer(), new Customer() });

        var result = await handler.Handle(new GetAllCustomersQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCustomerById_NotFound_ReturnsNull()
    {
        var handler = new GetCustomerByIdHandler(_mockRepo.Object);
        _mockRepo.Setup(x => x.GetByIdAsync("1")).ReturnsAsync((Customer?)null);

        var result = await handler.Handle(new GetCustomerByIdQuery("1"), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCustomer_Success()
    {
        var handler = new UpdateCustomerHandler(_mockRepo.Object, _mockUpdateLogger.Object);
        var customer = new Customer { Id = "1", Name = "Old" };
        _mockRepo.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(customer);

        var result = await handler.Handle(new UpdateCustomerCommand("1", "New", "0123456789", "Addr", CustomerType.REGULAR, 0), CancellationToken.None);

        result.Success.Should().BeTrue();
        customer.Name.Should().Be("New");
        _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomer_Success()
    {
        var handler = new DeleteCustomerHandler(_mockRepo.Object, _mockDeleteLogger.Object);

        var result = await handler.Handle(new DeleteCustomerCommand("1"), CancellationToken.None);

        result.Success.Should().BeTrue();
        _mockRepo.Verify(x => x.DeleteAsync("1"), Times.Once);
    }
}
