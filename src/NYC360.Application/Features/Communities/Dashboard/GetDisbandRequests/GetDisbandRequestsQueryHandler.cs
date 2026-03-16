using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Communities.Dashboard.GetDisbandRequests;

public class GetDisbandRequestsQueryHandler(ICommunityRepository communityRepository)
    : IRequestHandler<GetDisbandRequestsQuery, StandardResponse<PagedResponse<CommunityDisbandRequestDto>>>
{
    public async Task<StandardResponse<PagedResponse<CommunityDisbandRequestDto>>> Handle(GetDisbandRequestsQuery request, CancellationToken ct)
    {
        var (requests, total) = await communityRepository.GetDisbandRequestsPaginatedAsync(
            request.Status,
            request.Page,
            request.PageSize,
            ct);

        var dtos = requests.Select(CommunityDisbandRequestDtoExtensions.Map).ToList();

        var pagedResponse = PagedResponse<CommunityDisbandRequestDto>.Create(dtos, request.Page, request.PageSize, total);

        return StandardResponse<PagedResponse<CommunityDisbandRequestDto>>.Success(pagedResponse);
    }
}
