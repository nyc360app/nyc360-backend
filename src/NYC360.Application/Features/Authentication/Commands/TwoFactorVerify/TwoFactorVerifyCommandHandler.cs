using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.Application.Contracts.Authentication;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using NYC360.Domain.Entities.User;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.TwoFactorVerify;

public class TwoFactorVerifyCommandHandler(
    UserManager<ApplicationUser> userManager,
    IJwtGenerator jwtGenerator,
    IRefreshTokenRepository refreshTokenRepo,
    IUserClaimsGenerator claimsGenerator
) : IRequestHandler<TwoFactorVerifyCommand, StandardResponse<LoginResponse>>
{
    public async Task<StandardResponse<LoginResponse>> Handle(TwoFactorVerifyCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return StandardResponse<LoginResponse>.Failure(new("auth.notfound", "User not found."));

        var isValid = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, request.Code);
        if (!isValid)
            return StandardResponse<LoginResponse>.Failure(new("auth.invalid", "Invalid 2FA code."));

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