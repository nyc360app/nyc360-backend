using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Enums.Professions;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Professions.Commands.Withdraw;

public class WithdrawApplicationHandler(
    IProfessionsRepository professionsRepository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<WithdrawApplicationCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(WithdrawApplicationCommand request, CancellationToken ct)
    {
        // 1. Fetch and Verify Ownership
        var application = await professionsRepository.GetApplicationForApplicantAsync(
            request.ApplicationId, 
            request.UserId, 
            ct);

        if (application == null)
            return StandardResponse.Failure(new ApiError("app.not_found", "Application not found or access denied."));

        // 2. Validate current status
        if (application.Status == ApplicationStatus.Withdrawn)
            return StandardResponse.Failure(new ApiError("app.already_withdrawn", "This application is already withdrawn."));

        // 3. Update Status
        application.Status = ApplicationStatus.Withdrawn;
        application.StatusChangedAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}