using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Pages;

public record InterestGroupDto(Category Category, List<PostDto> Posts);

public record HomeFeedDto(
    List<PostDto> FeaturedPosts,
    List<InterestGroupDto> InterestGroups,
    List<PostDto> DiscoveryPosts,
    List<CommunityMinimalDto> SuggestedCommunities,
    List<string> TrendingTags
);