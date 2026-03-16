using NYC360.Application.Features.Authentication.Commands.Login;
using NYC360.Application.Contracts.Authentication;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Test.GetUserTokenById;

public class GetTokenByIdHandler(
    UserManager<ApplicationUser> userManager,
    IJwtGenerator jwtGenerator,
    IRefreshTokenRepository refreshTokenRepo,
    IUserClaimsGenerator claimsGenerator)
    : IRequestHandler<GetTokenByIdCommand, StandardResponse<LoginResponse>>
{
    public async Task<StandardResponse<LoginResponse>> Handle(GetTokenByIdCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            return StandardResponse<LoginResponse>.Failure(new ApiError("auth.invalid", "User not found."));
        }
        
        var customClaims = await claimsGenerator.GenerateUserClaimsAsync(user, ct);
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = await jwtGenerator.CreateTokenAsync(user, roles, customClaims, ct);

        var refreshToken = new RefreshToken
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