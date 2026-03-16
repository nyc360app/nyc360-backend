using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Entities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.RssSources.Queries.GetRequests;

public class GetRssConnectionRequestsQueryHandler(
    IRssFeedConnectionRequestRepository requestRepo)
    : IRequestHandler<GetRssConnectionRequestsQuery, PagedResponse<RssConnectionRequestDto>>
{
    public async Task<PagedResponse<RssConnectionRequestDto>> Handle(GetRssConnectionRequestsQuery request, CancellationToken cancellationToken)
    {
        var (items, count) = await requestRepo.GetPagedRequestsAsync(request.PageNumber, request.PageSize, request.Status, cancellationToken);

        var dtos = items.Select(x => new RssConnectionRequestDto(
                x.Id,
                x.Url,
                x.Category,
                x.Name,
                x.Description,
                x.ImageUrl,
                x.Status,
                x.AdminNote,
                x.RequesterId,
                x.Requester != null ? UserMinimalInfoDto.Map(x.Requester) : null,
                x.CreatedAt,
                x.ProcessedAt
            )).ToList();

        return PagedResponse<RssConnectionRequestDto>.Create(dtos, request.PageNumber, request.PageSize, count);
    }
}
