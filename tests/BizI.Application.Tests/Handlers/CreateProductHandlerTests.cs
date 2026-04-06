using System.Threading;
using System.Threading.Tasks;
using BizI.Application.Features.Orders;
using BizI.Application.Features.Products;
using BizI.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Tests;

public class CreateProductHandlerTests : BaseTest
{
    private readonly CreateProductHandler _handler;
    private readonly Mock<ILogger<CreateProductHandler>> _loggerMock;

    public CreateProductHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateProductHandler>>();
        _handler = new CreateProductHandler(ProductRepoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProductSuccessfully()
    {
        // Arrange
        var command = new CreateProductCommand("Test Product", 100);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeTrue();
        ProductRepoMock.Verify(x => x.AddAsync(It.Is<Product>(p => p.Name == command.Name && p.SalePrice == command.Price)), Times.Once);
    }
}
