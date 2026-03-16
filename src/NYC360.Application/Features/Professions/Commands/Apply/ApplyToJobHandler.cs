using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Professions;
using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Wrappers;
using MediatR;
using NYC360.Application.Contracts.Storage;

namespace NYC360.Application.Features.Professions.Commands.Apply;

public class ApplyToJobHandler(
    IProfessionsRepository professionsRepository,
    ILocalStorageService localStorageService,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<ApplyToJobCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(ApplyToJobCommand request, CancellationToken ct)
    {
        // 1. Fetch Job and Validate State
        var job = await professionsRepository.GetJobOfferByIdAsync(request.JobOfferId, ct);
        
        if (job == null)
            return StandardResponse<int>.Failure(new ApiError("job.not_found", "The job offer no longer exists."));

        if (!job.IsActive)
            return StandardResponse<int>.Failure(new ApiError("job.inactive", "This position is no longer accepting applications."));

        if (job.AuthorId == request.UserId)
            return StandardResponse<int>.Failure(new ApiError("job.owner_apply", "You cannot apply to your own job offer."));

        // 2. Check for existing application
        var alreadyApplied = await professionsRepository.HasAlreadyAppliedAsync(request.JobOfferId, request.UserId, ct);
        if (alreadyApplied)
            return StandardResponse<int>.Failure(new ApiError("job.duplicate_app", "You have already applied for this position."));

        // 3. Create Application
        var resumeUrl = request.Attachment != null 
            ? await localStorageService.SaveFileAsync(request.Attachment, "resumes", ct)
            : null;
        var application = new JobApplication
        {
            JobOfferId = request.JobOfferId,
            ApplicantId = request.UserId,
            CoverLetter = request.CoverLetter,
            ResumeUrl = resumeUrl,
            Status = ApplicationStatus.Pending,
            AppliedAt = DateTime.UtcNow
        };

        await professionsRepository.AddApplicationAsync(application, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<int>.Success(application.Id);
    }
}