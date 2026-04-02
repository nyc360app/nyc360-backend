using NYC360.Domain.Dtos.Posts;

namespace NYC360.Domain.Dtos.News;

public record NewsFeaturedFeedDto(
    List<PostDto> Items,
    string? NextCursor,
    bool HasMore
);
