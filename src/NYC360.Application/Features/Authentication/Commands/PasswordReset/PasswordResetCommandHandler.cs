using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.PasswordReset;

public class PasswordResetCommandHandler(
    UserManager<ApplicationUser> userManager) 
    : IRequestHandler<PasswordResetCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(PasswordResetCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return StandardResponse.Failure(new ApiError("auth.notfound", "User not found."));

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
            return StandardResponse.Failure(new ApiError("auth.resetfailed", string.Join("; ", result.Errors.Select(e => e.Description))));

        return StandardResponse.Success();
    }
}