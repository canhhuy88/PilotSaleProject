using BizI.Application.Features.Suppliers.Dtos;

namespace BizI.Application.Features.Suppliers.GetAll;

public record GetAllSuppliersQuery : IRequest<IEnumerable<SupplierDto>>;
