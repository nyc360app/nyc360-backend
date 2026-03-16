using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Tags;

namespace NYC360.Domain.Dtos.Pages;

public record DivisionHomeDto(
    List<PostDto> Featured,
    List<PostDto> Latest,
    List<PostDto> Trending,
    List<TagDto> Tags
);