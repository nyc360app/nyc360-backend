using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdatePosition;

public class UpdatePositionHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdatePositionCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdatePositionCommand request, CancellationToken ct)
    {
        // Fetch the specific position
        var position = await userRepository.GetPositionByIdAsync(request.PositionId, ct);
        
        // Security Check: Ensure position exists AND belongs to the calling user
        if (position == null || position.UserProfileId != request.UserId)
            return StandardResponse.Failure(new ApiError("position.not_found", "Position not found or access denied."));

        // Update properties
        position.Title = request.Title;
        position.Company = request.Company;
        position.StartDate = request.StartDate;
        position.EndDate = request.IsCurrent ? null : request.EndDate;
        position.IsCurrent = request.IsCurrent;

        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}