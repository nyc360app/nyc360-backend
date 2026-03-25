using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Constants;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetBySlug;

public class GetCommunityBySlugQueryHandler(
    ICommunityRepository communityRepository,
    ITagRepository tagRepository,
    IPostRepository postRepository)
    : IRequestHandler<GetCommunityBySlugQuery, StandardResponse<CommunityHomePageDto>>
{
    public async Task<StandardResponse<CommunityHomePageDto>> Handle(GetCommunityBySlugQuery request, CancellationToken ct)
    {
        var community = await communityRepository.GetBySlugAsync(request.Slug, ct);
        if (community is null)
        {
            return StandardResponse<CommunityHomePageDto>.Failure(
                new ApiError("community.notfound", "Community not found."));
        }

        var member = request.UserId.HasValue
            ? await communityRepository.GetMemberAsync(community.Id, request.UserId.Value, ct)
            : null;

        var leaderMember = community.Members
            .FirstOrDefault(m => m.Role == Domain.Enums.Communities.CommunityRole.Leader);

        var leader = leaderMember?.User is null
            ? null
            : new CommunityLeaderBasicDto(
                leaderMember.UserId,
                leaderMember.User.GetFullName(),
                leaderMember.User.AvatarUrl,
                leaderMember.User.User?.UserName);

        var isVerifiedLeader = leaderMember?.UserId is int leaderUserId &&
                               await tagRepository.UserHasTagAsync(
                                   leaderUserId,
                                   CommunityVerificationTags.ApplyForCommunityLeaderBadgesName,
                                   ct);

        var contentSummary = await postRepository.GetCommunityContentSummaryAsync(community.Id, ct);

        PagedResponse<PostDto>? posts = null;

        var canViewPosts = !community.IsPrivate || member != null;
        if (canViewPosts)
        {
            var (items, total) =
                await postRepository.GetAllByCommunityIdPaginatedAsync(
                    request.UserId,
                    community.Id,
                    request.Page,
                    request.PageSize,
                    ct);

            posts = PagedResponse<PostDto>.Create(
                items,
                request.Page,
                request.PageSize,
                total
            );
        }
        
        return StandardResponse<CommunityHomePageDto>.Success(
            new CommunityHomePageDto(
                CommunityDto.Map(community, leader, isVerifiedLeader, contentSummary),
                posts,
                member?.Role
            )
        );

    }
}
