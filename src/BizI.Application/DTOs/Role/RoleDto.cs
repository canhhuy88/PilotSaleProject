namespace BizI.Application.DTOs.Role;

public record RoleDto(Guid Id, string Name, IReadOnlyList<string> Permissions);

public record CreateRoleDto(string Name, List<string>? Permissions = null);

public record UpdateRoleDto(Guid Id, string Name, List<string>? Permissions = null);
