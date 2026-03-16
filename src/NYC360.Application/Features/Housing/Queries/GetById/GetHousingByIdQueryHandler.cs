using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Queries.GetById;

public class GetHousingByIdQueryHandler(
    IHouseInfoRepository housingRepository,
    IHousingRequestRepository requestRepository) 
    : IRequestHandler<GetHousingByIdQuery, StandardResponse<HousingDetailsDto>>
{
    public async Task<StandardResponse<HousingDetailsDto>> Handle(GetHousingByIdQuery request, CancellationToken ct)
    {
        var info = await housingRepository.GetHouseInfoByIdNoTrackingAsync(request.Id, ct);
        if (info == null)
            return StandardResponse<HousingDetailsDto>.Failure(new ApiError("housing.notfound", "Listing not found"));

        var userRequest = info.UserId != request.UserId 
            ? await requestRepository.GetUserSpecificPostRequestAsync(request.UserId!.Value, info.Id, ct) 
            : null;
        var similarEntities = await housingRepository.GetSimilarListingsAsync(info, 4, ct);
    
        var result = new HousingDetailsDto(
            HousingDto.Map(info), 
            info.HouseListingAuthorization?.Availabilities.Select(a => new AvailabilitySlotResponseDto(a.AvailabilityType, a.Dates, a.TimeFrom, a.TimeTo)).ToList()!,
            userRequest != null ? HousingRequestDto.Map(userRequest) : null,
            Similar: similarEntities.Select(HousingMinimalDto.Map).ToList()
        );

        return StandardResponse<HousingDetailsDto>.Success(result);
    }
}