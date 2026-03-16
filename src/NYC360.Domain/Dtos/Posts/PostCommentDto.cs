using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Dtos.User;

namespace NYC360.Domain.Dtos.Posts;

public sealed record PostCommentDto(
    int Id,
    string Content,
    UserMinimalInfoDto Author,
    DateTime CreatedAt,
    List<PostCommentDto> Replies
);

public static class PostCommentDtoExtensions
{
    extension(PostCommentDto)
    {
        public static PostCommentDto Map(PostComment comment)
        {
            return new PostCommentDto(
                comment.Id,
                comment.Content,
                UserMinimalInfoDto.Map(comment.User!),
                comment.CreatedAt,
                comment.Replies?.Select(PostCommentDto.Map).ToList() ?? new List<PostCommentDto>()
            );
        }
    }
}