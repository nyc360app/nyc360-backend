using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Commands.ProfileUpdates.AddEducation;

public class AddEducationCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<AddEducationCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(AddEducationCommand request, CancellationToken ct)
    {
        var profile = await userRepository.GetProfileByUserIdAsync(request.UserId, ct);
        
        if (profile == null)
            return StandardResponse<int>.Failure(new ApiError("profile.not_found", "Profile not found."));

        var education = new UserEducation
        {
            UserProfileId = request.UserId,
            School = request.School,
            Degree = request.Degree,
            FieldOfStudy = request.FieldOfStudy,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        profile.Educations.Add(education);
        
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(education.Id);
    }
}