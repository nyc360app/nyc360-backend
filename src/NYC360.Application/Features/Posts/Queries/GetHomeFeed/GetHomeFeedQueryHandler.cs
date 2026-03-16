using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Posts.Queries.GetHomeFeed;

public class GetHomeFeedHandler(
    IPostRepository postRepository,
    IUserRepository userRepository) 
    : IRequestHandler<GetHomeFeedQuery, StandardResponse<HomeFeedDto>>
{
    public async Task<StandardResponse<HomeFeedDto>> Handle(GetHomeFeedQuery request, CancellationToken ct)
    {
        var user = await userRepository.GetUserWithInterestsAsync(request.UserId, ct);
        if (user == null) return StandardResponse<HomeFeedDto>.Failure(new ApiError("user.not_found", "User not found"));

        var userInterests = user.Interests?.Select(i => i.Category).ToList() ?? new List<Category>();

        // 1. Fetch Featured Posts
        var featured = await postRepository.GetFeaturedPostsAsync(user.UserId, 4, ct);
    
        // Track IDs to prevent duplication
        var seenIds = featured.Select(p => p.Id).ToList();

        // 2. Fetch Grouped Interests (Excluding what's in Featured)
        var interestGroups = await postRepository.GetGroupedInterestPostsAsync(
            user.UserId, 
            userInterests, 
            10, 
            seenIds, 
            ct);

        // Add new IDs to the seen list
        seenIds.AddRange(interestGroups.SelectMany(g => g.Posts).Select(p => p.Id));

        // 3. Fetch Discovery (Excluding interests AND everything already seen)
        var discovery = await postRepository.GetDiscoveryPostsAsync(
            user.UserId, 
            userInterests, 
            seenIds, 
            6, 
            ct);

        // 4. Communities
        var communities = await postRepository.GetSuggestedCommunitiesAsync(
            request.UserId, 
            userInterests, 
            5, 
            ct
        );

        // 5. Fetch Trending Tags
        var trendingTags = await postRepository.GetTrendingTagsAsync(10, ct);
        
        return StandardResponse<HomeFeedDto>.Success(
            new HomeFeedDto(
                featured, 
                interestGroups, 
                discovery, 
                communities,
                trendingTags
            )
        );
    }
}