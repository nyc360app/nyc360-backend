using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums.Users;

namespace NYC360.Domain.Dtos.User;

public sealed record UserMinimalInfoDto(
    int Id, 
    string Username, 
    string FullName, 
    string? ImageUrl, 
    UserType Type
);

public static class UserPostDtoExtensions
{
    extension(UserMinimalInfoDto)
    {
        public static UserMinimalInfoDto Map(UserProfile user)
        {
            return new UserMinimalInfoDto(
                user.UserId, 
                user.User!.UserName!, 
                user.GetFullName(), 
                user.AvatarUrl, 
                user.User!.Type
            );
        }
    }
}