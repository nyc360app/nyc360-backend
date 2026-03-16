namespace NYC360.API.Models.Users;

public record UpdateUserPermissionsRequest(int UserId, List<string> Permissions);