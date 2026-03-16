using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.Application.Contracts.Authentication;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using NYC360.Domain.Entities.User;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepo,
    UserManager<ApplicationUser> userManager,
    IJwtGenerator jwtGenerator,
    IUserClaimsGenerator claimsGenerator)
    : IRequestHandler<RefreshTokenCommand, StandardResponse<LoginResponse>>
{
    public async Task<StandardResponse<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var refreshToken = await refreshTokenRepo.GetByTokenAsync(request.Token, ct);
        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.ExpiresAt < DateTime.UtcNow)
            return StandardResponse<LoginResponse>.Failure(new ApiError("auth.invalid", "Invalid or expired refresh token."));

        var user = await userManager.FindByIdAsync(refreshToken.UserId.ToString());
        if (user is null)
            return StandardResponse<LoginResponse>.Failure(new ApiError("auth.notfound", "User not found."));

        var customClaims = await claimsGenerator.GenerateUserClaimsAsync(user, ct);
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = await jwtGenerator.CreateTokenAsync(user, roles, customClaims, ct);

        // Rotate refresh token
        refreshToken.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        refreshToken.ExpiresAt = DateTime.UtcNow.AddDays(30);
        await refreshTokenRepo.SaveChangesAsync(ct);

        return StandardResponse<LoginResponse>.Success(new LoginResponse(accessToken, refreshToken.Token));
    }
}