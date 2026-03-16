using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Dtos.Pages;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Health.Queries.GetFeed;

public class GetHealthFeedHandler(IPostRepository postRepo) 
    : IRequestHandler<GetHealthFeedQuery, StandardResponse<HealthFeedDto>>
{
    public async Task<StandardResponse<HealthFeedDto>> Handle(GetHealthFeedQuery request, CancellationToken ct)
    {
        // 1. Fetch Health Articles (Hero + News Grid = 4 items)
        var (posts, _) = await postRepo.GetUniversalPostsAsync(
            request.UserId!.Value, 
            Category.Health,
            request.LocationId,
            null,
            null,
            1,
            4,
            ct);

        var hero = posts.FirstOrDefault();
        var news = posts.Skip(1).ToList();

        // 2. Fetch Initiatives
        // We use the new EventRepository we just started building
        var initiatives = await postRepo.GetUniversalPostsAsync(
            request.UserId!.Value, 
            Category.Health,
            request.LocationId,
            null,
            PostType.Initiative,
            1,
            2,
            ct);

        // 3. Fetch Wellness Grants (Posts tagged or categorized specifically)
        var (grants, _) = await postRepo.GetUniversalPostsAsync(
            request.UserId!.Value, 
            Category.Health,
            request.LocationId,
            null,
            PostType.Grant,
            1,
            4,
            ct);

        return StandardResponse<HealthFeedDto>.Success(new HealthFeedDto(
            hero,
            news,
            initiatives.Item1,
            grants
        ));
    }
}