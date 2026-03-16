using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityHomeDto(
    PagedResponse<PostDto> Feed,
    List<CommunityDiscoveryDto> Suggestions,
    List<TagDto> Tags
);