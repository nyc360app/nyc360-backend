using NYC360.Application.Contracts.Authentication;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Emails;
using NYC360.Application.Contracts.Jwt;
using NYC360.Application.Models.Emails;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler(
    UserManager<ApplicationUser> userManager,
    IUserRepository userRepository,
    IJwtGenerator jwtGenerator,
    IRefreshTokenRepository refreshTokenRepo,
    IEmailService emailService,
    IUserClaimsGenerator claimsGenerator)
    : IRequestHandler<LoginCommand, StandardResponse<LoginResponse>>
{
    public async Task<StandardResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            return StandardResponse<LoginResponse>.Failure(new ApiError("auth.invalid", "Invalid email or password."));

        // if (user.Profile == null)
        // {
        //     return StandardResponse<LoginResponse>.Failure(new("user.profile_not_complete", "Please complete your profile."));
        // }
         
        if (user.TwoFactorEnabled)
        {
            var otp = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            await emailService.SendOtpEmailAsync(user.Email!, new OtpEmailModel(user.GetFullName(), user.Email!, otp), ct);
            return StandardResponse<LoginResponse>.Success(new LoginResponse(null, null, true));
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
