namespace NYC360.API.Models.Roles;

public record EditRoleRequest(int RoleId, string Name, List<string> Permissions, int ContentLimit);