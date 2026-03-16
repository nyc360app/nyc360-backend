using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.Application.Contracts.Authentication;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Contracts.OAuth;
using NYC360.Application.Models.Emails;
using NYC360.Application.Contracts.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using NYC360.Domain.Entities.User;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.OAuthGoogle;

public sealed class OAuthLoginGoogleCommandHandler (
    UserManager<ApplicationUser> userManager,
    IJwtGenerator jwtGenerator,
    IOauthGoogleService oauthGoogleService,
    IRefreshTokenRepository refreshTokenRepo,
    IEmailService emailService,
    IUserClaimsGenerator claimsGenerator)
    : IRequestHandler<OAuthLoginGoogleCommand, StandardResponse<LoginResponse>>
{
    public async Task<StandardResponse<LoginResponse>> Handle(OAuthLoginGoogleCommand request, CancellationToken ct)
    {
        // get user data from google, if not found return error
        var oauthUser = await oauthGoogleService.GetPayloadAsync(request.IdToken);
        if (oauthUser == null)
        {
            return StandardResponse<LoginResponse>.Failure(new ApiError("auth.oauthgoogle", "failed to get user data from google."));
        }
        
        // retrieve user by email, if not found create new user
        var user = await userManager.FindByEmailAsync(oauthUser.Email);
        if (user == null)
        {
            var random = new Random();
            var userName = $"{oauthUser.GivenName}{oauthUser.FamilyName}{random.Next(0, 1000000)}".ToLower().Trim();
            user = new ApplicationUser
            {
                UserName = userName,
                Email = oauthUser.Email,
                EmailConfirmed = true,
                Profile = new UserProfile
                {
                    FirstName = oauthUser.GivenName,
                    LastName = oauthUser.FamilyName
                }
            };
            
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                var error = new ApiError("user.registration_failed", identityErrors);
                return StandardResponse<LoginResponse>.Failure(error);
            }
            await userManager.AddToRoleAsync(user, "User");

            var emailModel = new WelcomeEmailModel(user.GetFullName(), user.Email, string.Empty);
            await emailService.SendWelcomeEmailAsync(user.Email, emailModel, ct);
        }
        var customClaims = await claimsGenerator.GenerateUserClaimsAsync(user, ct);
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = await jwtGenerator.CreateTokenAsync(user, roles, customClaims, ct);

        var refreshToken = new Domain.Entities.RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        await refreshTokenRepo.AddAsync(refreshToken, ct);
        await refreshTokenRepo.SaveChangesAsync(ct);

        return StandardResponse<LoginResponse>.Success(new LoginResponse(accessToken, refreshToken.Token));
    }
}