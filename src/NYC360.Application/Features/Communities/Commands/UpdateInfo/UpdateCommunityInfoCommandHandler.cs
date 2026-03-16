using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Dtos.Communities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.UpdateInfo;

public class UpdateCommunityInfoCommandHandler(
    ICommunityRepository communityRepository,
    ILocationRepository locationRepository,
    ILocalStorageService localStorageService,
    ICommunityPermissionService permissionService,
    ISlugService slugService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCommunityInfoCommand, StandardResponse<CommunityDto>>
{
    public async Task<StandardResponse<CommunityDto>> Handle(UpdateCommunityInfoCommand request, CancellationToken ct)
    {
        // 1. Fetch the community
        var community = await communityRepository.GetByIdAsync(request.CommunityId, ct);
        if (community == null)
        {
            return StandardResponse<CommunityDto>.Failure(
                new ApiError("community.notFound", "Community not found."));
        }

        // 2. Authorization: Only the owner can update community info
        var isOwner = await permissionService.IsLeaderAsync(request.UserId, request.CommunityId, ct);
        if (!isOwner)
        {
            return StandardResponse<CommunityDto>.Failure(
                new ApiError("community.unauthorized", "Only the community owner can update community information."));
        }

        // 3. Validate LocationId if provided
        if (request.LocationId.HasValue && request.LocationId.Value != 0)
        {
            var locationExists = await locationRepository.ExistsAsync(request.LocationId.Value, ct);
            if (!locationExists)
            {
                return StandardResponse<CommunityDto>.Failure(
                    new ApiError("location.notFound", $"Location with ID {request.LocationId} does not exist."));
            }
        }

        // 4. Check for name uniqueness and update slug if name changed
        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != community.Name)
        {
            var nameExists = await communityRepository.IsNameAvailableAsync(request.Name, ct);
            if (nameExists)
            {
                return StandardResponse<CommunityDto>.Failure(
                    new ApiError("community.nameExists", "A community with this name already exists."));
            }

            community.Name = request.Name;
            // Regenerate slug based on new name
            community.Slug = await slugService.GenerateUniqueSlugAsync(
                request.Name, 
                s => communityRepository.SlugExistsAsync(s, ct), 
                ct);
        }

        // 5. Apply partial updates (only update non-null fields)
        if (!string.IsNullOrWhiteSpace(request.Description))
            community.Description = request.Description;

        if (request.Type.HasValue)
            community.Type = request.Type.Value;

        if (request.LocationId.HasValue)
            community.LocationId = request.LocationId.Value == 0 ? null : request.LocationId.Value;

        if (request.IsPrivate.HasValue)
            community.IsPrivate = request.IsPrivate.Value;

        if (request.RequiresApproval.HasValue)
            community.RequiresApproval = request.RequiresApproval.Value;

        // 6. Handle Avatar Image upload
        if (request.AvatarImage != null)
        {
            var avatarPath = await localStorageService.SaveFileAsync(request.AvatarImage, "communities", ct);
            community.AvatarUrl = "@local://" + avatarPath;
        }

        // 7. Handle Cover Image upload
        if (request.CoverImage != null)
        {
            var coverPath = await localStorageService.SaveFileAsync(request.CoverImage, "communities", ct);
            community.CoverUrl = "@local://" + coverPath;
        }

        // 8. Update timestamp
        community.LastUpdated = DateTime.UtcNow;

        // 9. Persist changes
        communityRepository.Update(community);
        await unitOfWork.SaveChangesAsync(ct);

        // 10. Return the updated DTO
        var result = CommunityDto.Map(community);
        return StandardResponse<CommunityDto>.Success(result);
    }
}
