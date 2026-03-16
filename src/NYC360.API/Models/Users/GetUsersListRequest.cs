namespace NYC360.API.Models.Users;

public record GetUsersListRequest(int Page = 1, int PageSize = 10, string? Search = null);