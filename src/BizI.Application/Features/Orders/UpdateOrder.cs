using MediatR;
using FluentValidation;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;
using System.Collections.Generic;

namespace BizI.Application.Features.Orders;

public record UpdateOrderCommand(string Id, decimal TotalAmount, BizI.Domain.Enums.OrderStatus Status, List<OrderItem> Items) : IRequest<CommandResult>;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, CommandResult>
{
    private readonly IRepository<Order> _repo;

    public UpdateOrderHandler(IRepository<Order> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.TotalAmount = request.TotalAmount;
        entity.Status = request.Status;
        entity.Items = request.Items;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
