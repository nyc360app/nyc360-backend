using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Tags;

namespace NYC360.Domain.Dtos.Housing;

public record HousingHomeDto(
    PostDto Hero,
    List<HousingMinimalDto> ForSale,
    List<HousingMinimalDto> ForRenting,
    List<PostDto> Rss,
    List<PostDto> Discussions,
    List<HousingMinimalDto> All,
    List<TagDto> Tags
);