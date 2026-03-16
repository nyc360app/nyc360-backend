using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetFeed;

public class GetHousingFeedHandler(IHouseInfoRepository housingRepository) 
    : IRequestHandler<GetHousingFeedQuery, PagedResponse<HousingMinimalDto>>
{
    public async Task<PagedResponse<HousingMinimalDto>> Handle(GetHousingFeedQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await housingRepository.GetPagedFeedAsync(
            request.PageNumber,
            request.PageSize,
            request.IsRenting,
            request.MinPrice,
            request.MaxPrice,
            request.LocationId,
            request.Search,
            ct);

        var dtos = items.Select(HousingMinimalDto.Map).ToList();

        return PagedResponse<HousingMinimalDto>.Create(
            dtos, 
            request.PageNumber, 
            request.PageSize, 
            totalCount);
    }
}