using MediatR;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetDisbandRequests;

public record GetDisbandRequestsQuery(
    int Page,
    int PageSize,
    DisbandRequestStatus? Status
) : IRequest<StandardResponse<PagedResponse<CommunityDisbandRequestDto>>>;
