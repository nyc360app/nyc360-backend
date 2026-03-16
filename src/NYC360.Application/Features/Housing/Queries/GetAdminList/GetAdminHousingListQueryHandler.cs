using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Housing.Queries.GetAdminList;

public class GetAdminHousingListQueryHandler(IHouseInfoRepository housingRepository) 
    : IRequestHandler<GetAdminHousingListQuery, PagedResponse<AgentListingDto>>
{
    public async Task<PagedResponse<AgentListingDto>> Handle(GetAdminHousingListQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await housingRepository.GetAdminPagedListingsAsync(
            request.PageNumber,
            request.PageSize,
            request.IsPublished,
            request.Search,
            ct);

        return PagedResponse<AgentListingDto>.Create(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}
