using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Tags;

namespace NYC360.Domain.Dtos.Professions;

public record ProfessionsFeedDto(
    PostDto? HeroArticle,
    List<PostDto> CareerArticles,
    List<JobOfferMinimalDto> HiringNews,
    List<TagDto> Tags
);