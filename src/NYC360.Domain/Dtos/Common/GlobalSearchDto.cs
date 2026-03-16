using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Dtos.Housing;

namespace NYC360.Domain.Dtos.Common;

public record GlobalSearchDto(
    List<PostDto>? Posts = null,
    List<UserSearchResultDto>? Users = null,
    List<CommunityDiscoveryDto>? Communities = null,
    List<TagDto>? Tags = null,
    List<HousingMinimalDto>? Housing = null
);