using NYC360.Domain.Entities.User;

namespace NYC360.Domain.Dtos.User;

public record UserStatsDto(
    int PostsCount = 0,
    int FollowersCount = 0,
    int FollowingCount = 0,
    int LikesCount = 0,
    int CommentsCount = 0,
    int SharesCount = 0,
    bool? IsVerified = null
);

public static class UserStatsDtoExtensions
{
    extension(UserStatsDto)
    {
        public static UserStatsDto? Map(UserStats? stats)
        {
            if (stats == null) 
                return null;
            
            return new UserStatsDto(
                stats.PostsCount,
                stats.FollowersCount,
                stats.FollowingCount,
                stats.LikesReceived,
                stats.CommentsCount,
                stats.Shares,
                stats.IsVerified
            );
        }
    }
}