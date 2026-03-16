using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Communities;
using NYC360.Domain.Enums.Communities;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.Create;

public class CreateCommunityCommandHandler(
    IUserRepository userRepository,
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
    // 1. Fetch User with nested dependencies
    var user = await userRepository.GetByIdWithStatsAsync(request.UserId, ct);
    
    // Check if User profile exists
    if (user == null)
    {
        return StandardResponse<string>.Failure(new ApiError("user.notFound", "User not found"));
    }

    // Check if the underlying Identity User is loaded
    if (user.User == null)
    {
        return StandardResponse<string>.Failure(new ApiError("user.identityMissing", "Identity data not loaded. Check repository includes."));
    }

    // 2. Safe Role & Verification Check
    var userRoles = await userManager.GetRolesAsync(user.User);
    bool isStaff = userRoles.Contains("SuperAdmin") || userRoles.Contains("Admin");
    // Use null-coalescing (?? false) instead of .Value to prevent crashes
    bool isVerified = user.Stats?.IsVerified ?? false;

    if (!isVerified && !isStaff)
    {
        return StandardResponse<string>.Failure(
            new ApiError("user.notVerified", "Only verified users can create communities")
        );
    }

    // 3. Location Check
    if (request.LocationId.HasValue && !await locationRepository.ExistsAsync(request.LocationId.Value, ct))
    {
        return StandardResponse<string>.Failure(new ApiError("location.notFound", "Location not found"));
    }
    
    // 4. Slug & Entity Creation
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
        // CRITICAL: Ensure this is initialized in the constructor or here
        Members = new List<CommunityMember>() 
    };
    
    entity.Members.Add(new CommunityMember
    {
        Community = entity,
        UserId = user.UserId, // Use the ID directly rather than the object to be safe
        Role = CommunityRole.Leader,
        JoinedAt = DateTime.UtcNow
    });

    // 5. Image Handling
    if (request.AvatarImage != null)
    {
        entity.AvatarUrl = await localStorageService.SaveFileAsync(request.AvatarImage, "communities", ct);
    }

    if (request.CoverImage != null)
    {
        entity.CoverUrl = await localStorageService.SaveFileAsync(request.CoverImage, "communities", ct);
    }

    // 6. Persist
    await communityRepository.AddAsync(entity, ct);
    await unitOfWork.SaveChangesAsync(ct);
    
    return StandardResponse<string>.Success(entity.Slug);
}
}