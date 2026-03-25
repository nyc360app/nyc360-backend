using NYC360.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Dtos.User;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Users.Queries.GetUserInfo;

public class GetUserInfoQueryHandler(
    IUserRepository userRepository,
    IVerificationRepository verificationRepository,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetUserInfoQuery, StandardResponse<UserInfoDto>>
{
    public async Task<StandardResponse<UserInfoDto>> Handle(GetUserInfoQuery request, CancellationToken ct)
    {
        // 1. Fetch User with all nested profile details
        var user = await userRepository.GetProfileInfoByUserIdAsync(request.UserId, ct);

        if (user == null) 
            return StandardResponse<UserInfoDto>.Failure(new ApiError("user.not_found", "User profile not found."));

        // 2. Map to UserInfoDto
        var roles = user.User is null
            ? new List<string>()
            : (await userManager.GetRolesAsync(user.User)).ToList();

        var dto = UserInfoDto.Map(user, roles);

        // Backward-compatible verification source:
        // some legacy accounts have approved identity requests but null/false stats flag.
        if (!dto.IsVerified)
        {
            var hasApprovedIdentity = await verificationRepository.HasApprovedIdentityRequestAsync(request.UserId, ct);
            if (hasApprovedIdentity)
                dto = dto with { IsVerified = true };
        }

        return StandardResponse<UserInfoDto>.Success(dto);
    }
}
