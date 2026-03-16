using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetRequests;

public class GetAgentRequestsQueryHandler(IHousingRequestRepository repository) 
    : IRequestHandler<GetAgentRequestsQuery, PagedResponse<HousingRequestDto>>
{
    public async Task<PagedResponse<HousingRequestDto>> Handle(GetAgentRequestsQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await repository.GetAgentPagedRequestsAsync(
            request.UserId,
            request.PageNumber, 
            request.PageSize,
            ct);
        
        var dtos = items.Select(HousingRequestDto.Map).ToList();
        
        return PagedResponse<HousingRequestDto>.Create(dtos, request.PageNumber, request.PageSize, totalCount);
    }
}