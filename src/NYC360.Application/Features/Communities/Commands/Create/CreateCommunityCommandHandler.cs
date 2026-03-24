using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.Create;

public class CreateCommunityCommandHandler(
    IUserRepository userRepository,
    ITagRepository tagRepository,
    ILocationRepository locationRepository,
    ICommunityRepository communityRepository,
    ILocalStorageService localStorageService,
    UserManager<ApplicationUser> userManager,
    ISlugService slugService,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateCommunityCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(CreateCommunityCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdWithStatsAsync(request.UserId, ct);
        if (user == null)
            return StandardResponse<string>.Failure(new ApiError("user.notFound", "User not found"));

        if (user.User == null)
            return StandardResponse<string>.Failure(new ApiError("user.identityMissing", "Identity data not loaded. Check repository includes."));

        var userRoles = await userManager.GetRolesAsync(user.User);
        var isStaff = userRoles.Contains("SuperAdmin") || userRoles.Contains("Admin");
        var isVerified = user.Stats?.IsVerified ?? false;

        if (!isVerified && !isStaff)
        {
            return StandardResponse<string>.Failure(
                new ApiError("user.notVerified", "Only verified users can create communities"));
        }

        // D01.2 gate: community creation requires the verified contributor tag, except staff.
        var hasCreateCommunityTag = isStaff || await tagRepository.UserHasTagAsync(
            request.UserId,
            CommunityVerificationTags.ApplyForCreateACommunityName,
            ct);

        if (!hasCreateCommunityTag)
        {
            return StandardResponse<string>.Failure(
                new ApiError(
                    "community.create.requiresContributorTag",
                    "You must be approved for 'Create a Community' before creating communities."));
        }

        if (request.LocationId.HasValue && !await locationRepository.ExistsAsync(request.LocationId.Value, ct))
            return StandardResponse<string>.Failure(new ApiError("location.notFound", "Location not found"));

        // Anti-spam duplicate check for same scope (name + type + location).
        var duplicateExists = await communityRepository.ExistsByNameTypeAndLocationAsync(
            request.Name,
            request.Type,
            request.LocationId,
            ct);
        if (duplicateExists)
        {
            return StandardResponse<string>.Failure(
                new ApiError(
                    "community.duplicate",
                    "A community with the same name, category, and location already exists."));
        }

        var slugSource = request.Slug ?? request.Name;
        var slug = await slugService.GenerateUniqueSlugAsync(slugSource, s => communityRepository.SlugExistsAsync(s, ct), ct);

        var entity = new Community
        {
            Name = request.Name,
            Slug = slug,
            Description = request.Description,
            LocationId = request.LocationId,
            Type = request.Type,
            IsPrivate = request.IsPrivate,
            Members = new List<CommunityMember>()
        };

        entity.Members.Add(new CommunityMember
        {
            Community = entity,
            UserId = user.UserId,
            Role = CommunityRole.Leader,
            JoinedAt = DateTime.UtcNow
        });

        if (request.AvatarImage != null)
            entity.AvatarUrl = await localStorageService.SaveFileAsync(request.AvatarImage, "communities", ct);

        if (request.CoverImage != null)
            entity.CoverUrl = await localStorageService.SaveFileAsync(request.CoverImage, "communities", ct);

        await communityRepository.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse<string>.Success(entity.Slug);
    }
}
