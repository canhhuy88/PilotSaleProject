using BizI.Application.Features.Users.Dtos;

namespace BizI.Application.Features.Users.GetAll;

public record GetAllUsersQuery : IRequest<IEnumerable<UserDto>>;
