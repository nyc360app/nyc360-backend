using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.ChangePassword;

public class ChangePasswordCommandHandler(
    UserManager<ApplicationUser> userManager,
    IEmailService emailService) 
    : IRequestHandler<ChangePasswordCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return StandardResponse.Failure(new ApiError("auth.notfound", "User not found."));

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
            return StandardResponse.Failure(new ApiError("auth.changefailed", string.Join("; ", result.Errors.Select(e => e.Description))));

        await emailService.SendPasswordChangedEmailAsync(user.Email!, new PasswordChangedEmailModel(user.GetFullName(), user.Email!), ct);
        return StandardResponse.Success();
    }
}