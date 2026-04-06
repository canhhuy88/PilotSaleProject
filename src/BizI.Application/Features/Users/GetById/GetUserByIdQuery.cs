using BizI.Application.Features.Users.Dtos;

namespace BizI.Application.Features.Users.GetById;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;
