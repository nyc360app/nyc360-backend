using NYC360.Domain.Entities.User;
using System.Security.Claims;

namespace NYC360.Application.Contracts.Jwt;

public interface IJwtGenerator
{
    Task<string> CreateTokenAsync(ApplicationUser user, IList<string> roles, List<Claim> customClaims, CancellationToken ct);
}