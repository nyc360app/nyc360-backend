namespace NYC360.Domain.Dtos.Communities;

public record CommunityContentSummaryDto(
    int TotalPosts,
    int EventPosts,
    int InitiativePosts,
    int NewsPosts
);
