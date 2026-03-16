using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.CreateAgentRequest;

public class CreateAgentRequestCommandHandler(
    IHousingRequestRepository housingRequestRepository,
    IHouseInfoRepository housingRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateAgentRequestCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateAgentRequestCommand request, CancellationToken ct)
    {
        var housingInfo = await housingRepository.GetByIdAsync(request.HouseInfoId, ct);
        if (housingInfo is null)
            return StandardResponse<int>.Failure(new ApiError("housing.post.notfound", "Post not found."));
        
        if(housingInfo.UserId == request.UserId) 
            return StandardResponse<int>.Failure(new ApiError("housing.post.author", "You cannot request your own post."));
        
        var housingRequest = new HousingRequest
        {
            UserId = request.UserId,
            HouseInfoId = request.HouseInfoId,
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PreferredContactType = request.PreferredContactType,
            PreferredContactDate = request.PreferredContactDate,
            HouseholdType = request.HouseholdType,
            MoveInDate = request.MoveInDate,
            MoveOutDate = request.MoveOutDate,
            ScheduleVirtualDate = request.ScheduleVirtualDate,
            ScheduleVirtualTimeWindow = request.ScheduleVirtualTimeWindow,
            InPersonTourDate = request.InPersonTourDate,
            InPersonTourTimeWindow = request.InPersonTourTimeWindow,
            Message = request.Message
        };

        await housingRequestRepository.AddAsync(housingRequest, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(housingRequest.Id);
    }
}