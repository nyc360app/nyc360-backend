using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Culture.Queries.GetStories;

public class GetCultureStoriesHandler(IPostRepository postRepository) 
    : IRequestHandler<GetCultureStoriesQuery, PagedResponse<PostDto>>
{
    public async Task<PagedResponse<PostDto>> Handle(GetCultureStoriesQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await postRepository.GetCultureStoriesPaginatedAsync(
            request.UserId == 0 ? null : request.UserId,
            request.Tag,
            request.TimeFrame,
            request.LocationId,
            request.Page,
            request.PageSize,
            ct
        );

        return PagedResponse<PostDto>.Create(
            items, 
            request.Page, 
            request.PageSize, 
            totalCount
        );
    }
}