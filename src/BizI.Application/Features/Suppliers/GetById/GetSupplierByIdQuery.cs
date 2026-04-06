using BizI.Application.Features.Suppliers.Dtos;

namespace BizI.Application.Features.Suppliers.GetById;

public record GetSupplierByIdQuery(Guid Id) : IRequest<SupplierDto?>;
