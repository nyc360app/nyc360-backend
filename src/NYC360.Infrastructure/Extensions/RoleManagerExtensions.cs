using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Extensions;

public static class RoleManagerExtensions
{
    public static async Task<bool> RoleHasClaimAsync(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string claimType, string claimValue)
    {
        var claims = await roleManager.GetClaimsAsync(role);
        return claims.Any(c => c.Type == claimType && c.Value == claimValue);
    }
}