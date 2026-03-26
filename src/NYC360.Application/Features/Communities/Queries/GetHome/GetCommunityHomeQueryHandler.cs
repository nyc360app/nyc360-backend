using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetHome;

public class GetCommunityHomeQueryHandler(
    ICommunityRepository communityRepository,
    IPostRepository postRepository,
    ITagRepository tagRepository)
    : IRequestHandler<GetCommunityHomeQuery, StandardResponse<CommunityHomeDto>>
{
    public async Task<StandardResponse<CommunityHomeDto>> Handle(GetCommunityHomeQuery request, CancellationToken ct)
    {
        // 1. Get IDs of communities the user has joined
        var joinedIds = await communityRepository.GetJoinedCommunityIdsAsync(request.UserId, ct);

        // The feed includes:
        // - Posts from communities the user has joined (all post types)
        // - Legacy Community-category posts that were created without CommunityId
        var (posts, total) = await postRepository.GetFeedByCommunityIdsAsync(request.UserId, joinedIds, request.Page, request.PageSize, ct);
        var feed = PagedResponse<PostDto>.Create(posts, request.Page, request.PageSize, total);

        // 2. Get Suggestions (Communities user hasn't joined yet)
        var suggestions = await communityRepository.GetSuggestionsAsync(request.UserId, 5, ct);
        var suggestionDtos = suggestions.Select(c => new CommunityDiscoveryDto(
            c.Id, c.Name, c.Slug, c.Description, c.AvatarUrl, c.Type ?? CommunityType.Neighborhood, c.Members.Count, c.IsPrivate
        )).ToList();

        // 3. Fetch Tags for Communities
        var tags = await tagRepository.GetTagsByDivisionAsync(Category.Community, ct);

        return StandardResponse<CommunityHomeDto>.Success(new CommunityHomeDto(
            feed, 
            suggestionDtos,
            tags.Select(t => t.Map()).ToList()));
    }
}
