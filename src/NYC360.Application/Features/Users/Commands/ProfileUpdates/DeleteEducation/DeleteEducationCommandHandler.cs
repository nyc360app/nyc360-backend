using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.DeleteEducation;

public class DeleteEducationCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteEducationCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteEducationCommand request, CancellationToken ct)
    {
        var education = await userRepository.GetEducationByIdAsync(request.EducationId, ct);
        
        if (education == null || education.UserProfileId != request.UserId)
            return StandardResponse.Failure(new ApiError("education.not_found", "Unauthorized."));

        userRepository.RemoveEducation(education);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}