using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.DeletePosition;

public class DeletePositionCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeletePositionCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeletePositionCommand request, CancellationToken ct)
    {
        var position = await userRepository.GetPositionByIdAsync(request.PositionId, ct);
        
        if (position == null || position.UserProfileId != request.UserId)
            return StandardResponse.Failure(new ApiError("position.not_found", "Position not found or unauthorized."));

        userRepository.RemovePosition(position);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}