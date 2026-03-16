using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetMyRequests;

public class GetMyRequestsQueryHandler(IHousingRequestRepository repository) 
    : IRequestHandler<GetMyRequestsQuery, PagedResponse<HousingRequestDto>>
{
    public async Task<PagedResponse<HousingRequestDto>> Handle(GetMyRequestsQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await repository.GetUserPagedRequestsAsync(
            request.UserId,
            request.PageNumber, 
            request.PageSize,
            ct);
        
        var dtos = items.Select(HousingRequestDto.Map).ToList();
        
        return PagedResponse<HousingRequestDto>.Create(dtos, request.PageNumber, request.PageSize, totalCount);
    }
}