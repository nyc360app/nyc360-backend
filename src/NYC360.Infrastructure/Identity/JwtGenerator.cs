using Microsoft.Extensions.Configuration;
using NYC360.Application.Contracts.Jwt;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities.User;  
using System.Security.Claims;
using FastEndpoints.Security;

namespace NYC360.Infrastructure.Identity;

public class JwtGenerator(IConfiguration config, RoleManager<ApplicationRole> roleManager) : IJwtGenerator
{
    public async Task<string> CreateTokenAsync(ApplicationUser user, IList<string> roles, List<Claim> customClaims, CancellationToken ct)
    {
        try
        {
            List<string> permissions = [];

            foreach (var roleName in roles)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null) 
                    continue;
                
                var roleClaims = await roleManager.GetClaimsAsync(role);
                permissions.AddRange(roleClaims.Select(c => c.Value));
            }
            return JwtBearer.CreateToken(o =>
            {
                o.SigningKey = config["Jwt:Key"]!;
                o.ExpireAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(config["Jwt:AccessTokenExpirationInMinutes"]));
                
                o.User.Claims.Add((ClaimTypes.NameIdentifier, user.Id.ToString()));
                o.User.Claims.Add((ClaimTypes.Name, user.UserName!));
                o.User.Claims.Add((ClaimTypes.Email, user.Email!));
                o.User.Claims.Add(("UserType", user.Type.ToString()));
                
                foreach (var claim in customClaims)
                {
                    o.User.Claims.Add((claim.Type, claim.Value));
                }
                
                foreach (var role in roles)
                {
                    o.User.Claims.Add((ClaimTypes.Role, role));
                    o.User.Roles.Add(role);
                }
                o.User.Permissions.AddRange(permissions);
            });
        }
        catch
        {
            return string.Empty;
        }
    }
}