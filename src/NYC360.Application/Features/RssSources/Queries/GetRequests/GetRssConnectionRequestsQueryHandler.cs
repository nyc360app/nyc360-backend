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
        var (items, count) = await requestRepo.GetPagedRequestsAsync(request.PageNumber, request.PageSize, request.Status, request.Category, cancellationToken);

        var dtos = items.Select(x =>
            {
                var requester = x.Requester != null ? UserMinimalInfoDto.Map(x.Requester) : null;

                return new RssConnectionRequestDto(
                    x.Id,
                    x.Url,
                    x.Category,
                    x.Category,
                    x.FinalCategory,
                    x.Name,
                    x.Description,
                    x.ImageUrl,
                    x.LogoImageUrl,
                    x.LogoFileName,
                    x.Language,
                    x.SourceWebsite,
                    x.SourceCredibility,
                    x.AgreementAccepted,
                    x.DivisionTag,
                    x.Status,
                    x.AdminNote,
                    x.RequesterId,
                    requester?.FullName,
                    requester,
                    x.ProcessedByUserId,
                    x.CreatedAt,
                    x.ProcessedAt
                );
            })
            .ToList();

        return PagedResponse<RssConnectionRequestDto>.Create(dtos, request.PageNumber, request.PageSize, count);
    }
}
