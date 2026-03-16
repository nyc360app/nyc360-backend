using NYC360.Domain.Entities.Posts;

namespace NYC360.Domain.Dtos.Posts;

public record PostStatsDto(
    int Views,
    int Likes,
    int Dislikes,
    int Comments,
    int Shares
);

public static class PostStatsDtoExtensions
{
    extension(PostStatsDto)
    {
        public static PostStatsDto Map(PostStats stats)
        {
            return new PostStatsDto(
                stats.Views,
                stats.Likes,
                stats.Dislikes,
                stats.Comments,
                stats.Shares
            );
        }
    }
}