using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BizI.Application.Features.Categories;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BizI.Application.Tests.Features.Categories;

public class CategoryHandlerTests
{
    private readonly Mock<IRepository<Category>> _mockRepo;
    private readonly Mock<ILogger<CreateCategoryHandler>> _mockCreateLogger;
    private readonly Mock<ILogger<UpdateCategoryHandler>> _mockUpdateLogger;
    private readonly Mock<ILogger<DeleteCategoryHandler>> _mockDeleteLogger;

    public CategoryHandlerTests()
    {
        _mockRepo = new Mock<IRepository<Category>>();
        _mockCreateLogger = new Mock<ILogger<CreateCategoryHandler>>();
        _mockUpdateLogger = new Mock<ILogger<UpdateCategoryHandler>>();
        _mockDeleteLogger = new Mock<ILogger<DeleteCategoryHandler>>();
    }

    [Fact]
    public async Task CreateCategory_Success()
    {
        var handler = new CreateCategoryHandler(_mockRepo.Object, _mockCreateLogger.Object);
        var command = new CreateCategoryCommand("Test", null, "Desc");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.Should().BeTrue();
        _mockRepo.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsData()
    {
        var handler = new GetAllCategoriesHandler(_mockRepo.Object);
        _mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Category> { new Category(), new Category() });

        var result = await handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCategoryById_NotFound_ReturnsNull()
    {
        var handler = new GetCategoryByIdHandler(_mockRepo.Object);
        _mockRepo.Setup(x => x.GetByIdAsync("1")).ReturnsAsync((Category?)null);

        var result = await handler.Handle(new GetCategoryByIdQuery("1"), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateCategory_Success()
    {
        var handler = new UpdateCategoryHandler(_mockRepo.Object, _mockUpdateLogger.Object);
        var category = new Category { Id = "1", Name = "Old" };
        _mockRepo.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(category);

        var result = await handler.Handle(new UpdateCategoryCommand("1", "New", null, null), CancellationToken.None);

        result.Success.Should().BeTrue();
        category.Name.Should().Be("New");
        _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCategory_Success()
    {
        var handler = new DeleteCategoryHandler(_mockRepo.Object, _mockDeleteLogger.Object);

        var result = await handler.Handle(new DeleteCategoryCommand("1"), CancellationToken.None);

        result.Success.Should().BeTrue();
        _mockRepo.Verify(x => x.DeleteAsync("1"), Times.Once);
    }
}
