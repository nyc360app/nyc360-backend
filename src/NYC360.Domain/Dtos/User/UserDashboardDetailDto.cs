using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Dtos.User;

public sealed record UserDashboardDetailDto(
    int Id,
    string? Name,
    string Username,
    string Email,
    bool EmailConfirmed,
    string? ImageUrl,
    string Role,
    bool LockoutEnabled,
    DateTimeOffset? LockoutEnd
);

public static class UserDashboardDetailDtoExtension
{
    extension(UserDashboardDetailDto)
    {
        public static UserDashboardDetailDto Map(ApplicationUser entity, IList<string> roles)
        {
            return new UserDashboardDetailDto(
                entity.Id,
                entity.GetFullName(),
                entity.UserName!,
                entity.Email!,
                entity.EmailConfirmed,
                entity.Profile!.AvatarUrl ?? string.Empty,
                roles.FirstOrDefault() ?? "User",
                LockoutEnabled: entity.LockoutEnabled,
                LockoutEnd: entity.LockoutEnd
            );
        }
    }
}