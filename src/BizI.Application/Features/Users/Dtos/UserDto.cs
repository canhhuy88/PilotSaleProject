namespace BizI.Application.Features.Users.Dtos;

public record UserDto(
    Guid Id,
    string Username,
    string FullName,
    Guid RoleId,
    bool IsActive);

public record CreateUserDto(
    string Username,
    string Password,
    string FullName,
    Guid RoleId);

public record UpdateUserDto(
    Guid Id,
    string FullName,
    Guid RoleId);
