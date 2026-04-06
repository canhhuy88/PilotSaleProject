using BizI.Application.Features.Roles.Dtos;

namespace BizI.Application.Features.Roles.GetAll;

public record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;
