using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Entities;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Verifications.Commands.TagRequest;

public class SubmitTagVerificationCommandHandler(
    ITagRepository tagRepository,
    IVerificationRepository verificationRepository, // New Repo for the Requests table
    IUnitOfWork unitOfWork,
    ILocalStorageService storageService)
    : IRequestHandler<SubmitTagVerificationCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(SubmitTagVerificationCommand request, CancellationToken ct)
    {
        // 1. Verify Tag exists and is Verifiable (Identity or Professional)
        var tag = await tagRepository.GetByIdAsync(request.TagId);
        if (tag == null || (tag.Type != TagType.Identity && tag.Type != TagType.Professional))
        {
            return StandardResponse.Failure(new ApiError("tag.not_found", "Tag not found."));
        }

        // 2. Check if user already has this tag
        var alreadyHasTag = await verificationRepository.UserHasSpecificTagAsync(request.UserId, request.TagId, ct);
        if (alreadyHasTag) return StandardResponse.Failure(new ApiError("tag.already_exists", "User already has this tag."));

        // 3. Check for existing pending request
        var hasPending = await verificationRepository.HasPendingRequestAsync(request.UserId, request.TagId, ct);
        if (hasPending) return StandardResponse.Failure(new ApiError("tag.verification_pending", "A verification request for this tag already exists."));

        // 4. Save file to PRIVATE storage (not public wwwroot)
        // We use a specific folder for verifications
        var fileName = await storageService.SaveFileAsync(request.File, "verifications", ct);

        // 5. Create Entity
        var verificationRequest = new TagVerificationRequest
        {
            UserId = request.UserId,
            TargetTagId = request.TagId,
            Reason = request.Reason,
            Status = VerificationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        verificationRequest.Documents.Add(new VerificationDocument
        {
            DocumentType = request.DocumentType,
            FileUrl = fileName
        });

        await verificationRepository.AddAsync(verificationRequest, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}