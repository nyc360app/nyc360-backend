using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateBasicInfo;

public class UpdateBasicInfoCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateBasicInfoCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateBasicInfoCommand request, CancellationToken ct)
    {
        // Fetch the profile specifically (not the full Identity User) for better performance
        var profile = await userRepository.GetProfileByUserIdAsync(request.UserId, ct);
        
        if (profile == null)
            return StandardResponse.Failure(new ApiError("profile.not_found", "User profile not found."));

        // Update fields
        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;
        profile.Headline = request.Headline;
        profile.Bio = request.Bio;
        profile.AddressId = request.LocationId;

        // Persist changes
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}