namespace NYC360.API.Models.Users;

public record UpdateUserRolesRequest(int UserId, string RoleName);