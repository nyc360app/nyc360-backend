using NYC360.Domain.Dtos.Posts;

namespace NYC360.Domain.Dtos.Pages;

public record HealthFeedDto(
    PostDto? HeroArticle,
    List<PostDto> HealthNews,
    List<PostDto> Initiatives,
    List<PostDto> WellnessGrants
);