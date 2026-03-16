using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Queries.GetBySlug;

public class GetCommunityBySlugQueryHandler(
    ICommunityRepository communityRepository,
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

        var member = await communityRepository.GetMemberAsync(community.Id, request.UserId, ct);

        PagedResponse<PostDto>? posts = null;

        if (member != null)
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
                CommunityDto.Map(community),
                posts,
                member?.Role
            )
        );

    }
}