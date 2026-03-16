using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Users.Commands.ToggleTwoFactor;

public class ToggleTwoFactorCommandHandler(
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<ToggleTwoFactorCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ToggleTwoFactorCommand request, CancellationToken ct)
    {
        // Get current user from context (you might pass userId via JWT claim)
        // Assume userId is passed via command, or get it in endpoint
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            return StandardResponse.Failure(new ApiError("users.notfound", "User not found."));

        var result = await userManager.SetTwoFactorEnabledAsync(user, request.Enable);
        if (!result.Succeeded)
            return StandardResponse.Failure(new ApiError("users.2fa.failed", 
                string.Join("; ", result.Errors.Select(e => e.Description))));

        return StandardResponse.Success();
    }
}