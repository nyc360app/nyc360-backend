namespace NYC360.Domain.Dtos.Posts;

public record PostAnalysisDto(
    int TotalPosts,
    long TotalViews,
    long TotalLikes,
    long TotalDislikes,
    long TotalComments,
    long TotalShares,
    double EngagementRate,
    PostMinimalDto? TopPost
);
