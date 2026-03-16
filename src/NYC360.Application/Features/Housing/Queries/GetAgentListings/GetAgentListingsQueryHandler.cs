using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetAgentListings;

public class GetAgentListingsQueryHandler(IHouseInfoRepository repository) 
    : IRequestHandler<GetAgentListingsQuery, PagedResponse<AgentListingDto>>
{
    public async Task<PagedResponse<AgentListingDto>> Handle(GetAgentListingsQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await repository.GetAgentPagedListingsAsync(
            request.UserId,
            request.PageNumber,
            request.PageSize,
            ct);

        return PagedResponse<AgentListingDto>.Create(items, request.PageNumber, request.PageSize, totalCount);
    }
}
