using BizI.Application.Features.Roles.Dtos;

namespace BizI.Application.Features.Roles.GetById;

public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto?>;
