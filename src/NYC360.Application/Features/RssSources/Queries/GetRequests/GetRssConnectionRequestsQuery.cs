using MediatR;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Queries.GetRequests;

public record GetRssConnectionRequestsQuery(
    int PageNumber, 
    int PageSize, 
    RssConnectionStatus? Status,
    Category? Category = null) : IRequest<PagedResponse<RssConnectionRequestDto>>;
