using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Entities;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Verifications.Commands.SubmitIdentity;

public class SubmitIdentityVerificationHandler(
    IVerificationRepository verificationRepository,
    ITagRepository tagRepository,
    UserManager<ApplicationUser> userManager,
    ILocalStorageService storageService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SubmitIdentityVerificationCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(SubmitIdentityVerificationCommand request, CancellationToken ct)
    {
        // 1. Determine the Tag based on Role
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        var roles = await userManager.GetRolesAsync(user!);
        var roleName = roles.FirstOrDefault();

        // Map Role to Tag ID (Based on your Tags.csv)
        var targetTagId = roleName switch
        {
            "Organization" => 3, // Organization Tag
            "Business"     => 2, // Business Tag
            _              => 1  // New Yorker Tag (Default for Visitors/New Yorkers)
        };

        // 2. Security Checks
        if (await verificationRepository.UserHasSpecificTagAsync(request.UserId, targetTagId, ct))
            return StandardResponse.Failure(new ApiError("verify.already_verified", "You already have this identity verified."));

        if (await verificationRepository.HasPendingRequestAsync(request.UserId, targetTagId, ct))
            return StandardResponse.Failure(new ApiError("verify.pending", "Your verification is already under review."));

        // 3. Document Save (Private Storage)
        var filePath = await storageService.SaveFileAsync(request.File, "verifications", ct);

        // 4. Create the Request
        var vRequest = new TagVerificationRequest
        {
            UserId = request.UserId,
            TargetTagId = targetTagId, // Set by Backend, not User
            Reason = request.Reason,
            Status = VerificationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        vRequest.Documents.Add(new VerificationDocument
        {
            DocumentType = request.DocumentType,
            FileUrl = filePath
        });

        await verificationRepository.AddAsync(vRequest, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}