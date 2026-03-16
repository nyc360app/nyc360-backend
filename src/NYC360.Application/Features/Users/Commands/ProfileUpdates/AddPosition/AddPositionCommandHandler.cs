using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.AddPosition;

public class AddPositionCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<AddPositionCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(AddPositionCommand request, CancellationToken ct)
    {
        var profile = await userRepository.GetProfileByUserIdAsync(request.UserId, ct);
        
        if (profile == null)
            return StandardResponse<int>.Failure(new ApiError("profile.not_found", "Profile not found."));

        var position = new UserPosition
        {
            UserProfileId = request.UserId, // Linked to the profile PK
            Title = request.Title,
            Company = request.Company,
            StartDate = request.StartDate,
            EndDate = request.IsCurrent ? null : request.EndDate,
            IsCurrent = request.IsCurrent
        };

        profile.Positions.Add(position);
        
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(position.Id);
    }
}