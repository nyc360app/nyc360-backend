using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Constants;
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
        // Accept the stable tag IDs and gracefully resolve known legacy community IDs by name.
        var tag = await ResolveVerifiableTagAsync(request.TagId, ct);
        if (tag == null)
        {
            return StandardResponse.Failure(new ApiError("tag.not_found", "Tag not found."));
        }

        var tagId = tag.Id;

        // 2. Check if user already has this tag
        var alreadyHasTag = await verificationRepository.UserHasSpecificTagAsync(request.UserId, tagId, ct);
        if (alreadyHasTag) return StandardResponse.Failure(new ApiError("tag.already_exists", "User already has this tag."));

        // 3. Check for existing pending request
        var hasPending = await verificationRepository.HasPendingRequestAsync(request.UserId, tagId, ct);
        if (hasPending) return StandardResponse.Failure(new ApiError("tag.verification_pending", "A verification request for this tag already exists."));

        // 4. Save file to PRIVATE storage (not public wwwroot)
        // We use a specific folder for verifications
        var fileName = await storageService.SaveFileAsync(request.File, "verifications", ct);

        // 5. Create Entity
        var verificationRequest = new TagVerificationRequest
        {
            UserId = request.UserId,
            TargetTagId = tagId,
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

    private async Task<Tag?> ResolveVerifiableTagAsync(int requestedTagId, CancellationToken ct)
    {
        var tag = await tagRepository.GetByIdAsync(requestedTagId);
        if (IsVerifiable(tag))
            return tag;

        var tagName = ResolveStableTagName(requestedTagId);
        if (tagName == null)
            return null;

        tag = await tagRepository.GetByNameAsync(tagName, ct);
        return IsVerifiable(tag) ? tag : null;
    }

    private static bool IsVerifiable(Tag? tag)
        => tag is not null && (tag.Type == TagType.Identity || tag.Type == TagType.Professional);

    private static string? ResolveStableTagName(int tagId)
    {
        if (CommunityVerificationTags.TryGetName(tagId, out var communityTagName))
            return communityTagName;

        if (NewsDepartmentTags.TryGetName(tagId, out var newsTagName))
            return newsTagName;

        return null;
    }
}
