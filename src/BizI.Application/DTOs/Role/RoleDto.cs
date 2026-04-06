namespace BizI.Application.DTOs.Role;

public record RoleDto(string Id, string Name, IReadOnlyList<string> Permissions);

public record CreateRoleDto(string Name, List<string>? Permissions = null);

public record UpdateRoleDto(string Id, string Name, List<string>? Permissions = null);
