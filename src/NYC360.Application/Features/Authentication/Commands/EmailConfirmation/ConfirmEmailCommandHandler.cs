using NYC360.Application.Contracts.Emails;
using NYC360.Application.Models.Emails;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.EmailConfirmation;

public class ConfirmEmailCommandHandler(
    UserManager<ApplicationUser> userManager,
    IEmailService emailService)
    : IRequestHandler<ConfirmEmailCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return StandardResponse.Failure(new ApiError("auth.user_not_found", "User not found."));

        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            return StandardResponse.Failure(new ApiError("auth.confirmation_failed", "Email confirmation failed."));
        }
        
        var model = new WelcomeEmailModel(user.GetFullName(), user.Email!, string.Empty);
        await emailService.SendVerificationSuccessEmailAsync(user.Email!, model, ct);
        
        return StandardResponse.Success();
    }
}