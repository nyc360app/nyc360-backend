using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateEducation;

public class UpdateEducationCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateEducationCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateEducationCommand request, CancellationToken ct)
    {
        var education = await userRepository.GetEducationByIdAsync(request.EducationId, ct);
        
        if (education == null || education.UserProfileId != request.UserId)
            return StandardResponse.Failure(new ApiError("education.not_found", "Education record not found."));

        education.School = request.School;
        education.Degree = request.Degree;
        education.FieldOfStudy = request.FieldOfStudy;
        education.StartDate = request.StartDate;
        education.EndDate = request.EndDate;

        await unitOfWork.SaveChangesAsync(ct);
        return StandardResponse.Success();
    }
}