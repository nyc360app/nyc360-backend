using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Enums;

namespace NYC360.Application.Contracts.Persistence;

public interface IPostRepository
{
    Task AddAsync(Post post, CancellationToken ct);
    Task AddAsync(List<Post> posts, CancellationToken ct);
    
    Task<Post?> GetByIdAsync(int id, CancellationToken ct);
    Task<Post?> GetByIdWithStatsAsync(int postId, CancellationToken ct);
    Task<PostDto?> GetPostByIdAsync(int id, int? userId, CancellationToken ct);
    Task<PostDetailsDto?> GetPostWithDetailsDtoByIdAsync(int id, int? userId, bool includeUnapproved, CancellationToken ct);
    Task<(List<PostDto>, int)> GetAllPaginatedAsync(int page, int pageSize, int? userId, Category? category, string? search, PostType? postType, PostSource? sourceType, int? authorId, bool includeUnapproved, CancellationToken ct);
    Task<(List<PostDto>, int)> GetAllByCommunityIdPaginatedAsync(int? UserId, int communityId, int page, int pageSize, CancellationToken ct);
    Task<CommunityContentSummaryDto> GetCommunityContentSummaryAsync(int communityId, CancellationToken ct);
    Task<(List<PostDto>, int)> GetTrendingPaginatedAsync(List<Category> userInterests, int page, int pageSize, int? userId, CancellationToken ct);
    Task<(List<PostDto>, int)> GetFeedByCommunityIdsAsync(int? userId, List<int> communityIds, int page, int pageSize, CancellationToken ct);
    Task<List<PostDto>> GetFeaturedPostsAsync(int? userId, int count, CancellationToken ct);
    Task<List<InterestGroupDto>> GetGroupedInterestPostsAsync(int? userId, List<Category> categories, int postsPerCategory, List<int> excludeIds, CancellationToken ct);
    Task<List<PostDto>> GetDiscoveryPostsAsync(int? userId, List<Category> excludedCategories, List<int> excludeIds, int count, CancellationToken ct);
    Task<List<CommunityMinimalDto>> GetSuggestedCommunitiesAsync(int userId, List<Category> userInterests, int count, CancellationToken ct);
    Task<List<PostDto>> GetRecentUserPostsAsync(int userId, int count, CancellationToken ct);
    Task<List<PostDto>> GetLatestPostsAsync(Category? division, int? userId, int limit, CancellationToken ct);
    Task<List<PostDto>> GetFeaturedPostsAsync(Category? division, int? userId, int limit, CancellationToken ct);
    Task<List<PostDto>> GetTrendingPostsAsync(Category? division, int? userId, int limit, CancellationToken ct);
    Task<List<PostDto>> GetFeaturedNewsFeedAsync(int? userId, int pageSize, int page, DateTime? cursorTime, int? cursorId, int take, CancellationToken ct);
    Task<(List<PostDto>, int)> GetSavedPostsPaginatedAsync(int userId, int page, int pageSize, Category? category, CancellationToken ct);
    
    Task<(List<PostDto>, int)> GetCultureStoriesPaginatedAsync(int? userId, string? tag, string? timeFrame, int? locationId, int page, int pageSize, CancellationToken ct);

    Task<(List<PostDto>, int)> GetUniversalPostsAsync(int userId, Category? category, int? locationId, string? search, PostType? type, int page,
        int pageSize, CancellationToken ct);

    Task<List<PostDto>> SearchPostsAsync(string term, Category? div, int? userId, int limit, CancellationToken ct);
    Task<(List<NewsSubmissionDto>, int)> GetPendingNewsSubmissionsAsync(int page, int pageSize, string? search, CancellationToken ct);
    
    // Tags
    Task<List<Tag>> EnsureTagsExistAsync(List<string> tagNames, CancellationToken ct);
    Task<List<string>> GetTrendingTagsAsync(int count, CancellationToken ct);
    Task<(List<PostDto>, int)> GetPostsByTagPaginatedAsync(string tagName, int page, int pageSize, int? userId, CancellationToken ct);
    Task<(List<PostDto>, int)> GetPostsByTopicPaginatedAsync(int topicId, int page, int pageSize, int? userId, CancellationToken ct);
    Task<List<string>> GetExistingLinksAsync(List<string> links, CancellationToken ct);
    Task<bool> ExistsAsync(int id, CancellationToken ct);
    void Update(Post post);
    void Remove(Post post);
    
    // Housing
    Task<List<PostDto>> GetRecentHousingPostsAsync(int? userId, int limit, CancellationToken ct);
    Task<PostAnalysisDto> GetMyPostsAnalysisByCategoryAsync(int userId, Category category, CancellationToken ct);
}
