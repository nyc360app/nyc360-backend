using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Domain.Dtos.Communities;

public record CommunityHomePageDto(
    CommunityDto Community,
    PagedResponse<PostDto>? Posts,
    CommunityRole? MemberRole
);