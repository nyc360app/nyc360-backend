using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(
    UserManager<ApplicationUser> userManager,
    IEmailService emailService
    ) 
    : IRequestHandler<ForgotPasswordCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ForgotPasswordCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return StandardResponse.Failure(new ApiError("auth.notfound", "User not found."));

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var emailModel = new ResetPasswordEmailModel(user.GetFullName(), user.Email!, token);
        await emailService.SendResetPasswordEmailAsync(user.Email!, emailModel, ct);

        return StandardResponse.Success();
    }
}