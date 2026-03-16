using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.UpdateApplicationStatus;

public class UpdateApplicationStatusHandler(
    IProfessionsRepository professionsRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateApplicationStatusCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateApplicationStatusCommand request, CancellationToken ct)
    {
        // 1. Fetch Application & Verify Ownership
        var application = await professionsRepository.GetApplicationForOwnerAsync(
            request.ApplicationId, 
            request.UserId, 
            ct);
        
        if (application == null)
            return StandardResponse.Failure(new ApiError("app.not_found", "Application not found or access denied."));

        // 2. Business Rule Checks
        if (application.Status == ApplicationStatus.Withdrawn)
            return StandardResponse.Failure(new ApiError("app.withdrawn", "Cannot update an application that has been withdrawn by the candidate."));

        // 3. Update State
        application.Status = request.NewStatus;
        application.StatusChangedAt = DateTime.UtcNow;

        // 4. Persist
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}