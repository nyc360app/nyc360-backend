namespace NYC360.API.Models.Roles;

public sealed record CreateRoleRequest(string Name, List<string> Permissions, int ContentLimit);