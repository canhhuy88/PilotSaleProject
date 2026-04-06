namespace BizI.Application.DTOs.User;

public record UserDto(
    string Id,
    string Username,
    string FullName,
    string RoleId,
    bool IsActive);

public record CreateUserDto(
    string Username,
    string Password,
    string FullName,
    string RoleId);

public record UpdateUserDto(
    string Id,
    string FullName,
    string RoleId);
