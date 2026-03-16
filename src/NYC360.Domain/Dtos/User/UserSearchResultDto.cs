namespace NYC360.Domain.Dtos.User;

public record UserSearchResultDto(
    int Id,
    string UserName,
    string? FullName,
    string? ImageUrl,
    string IdentityTag
);