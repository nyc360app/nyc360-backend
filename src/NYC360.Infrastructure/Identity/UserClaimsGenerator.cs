using NYC360.Application.Contracts.Authentication;
using NYC360.Application.Contracts.Persistence;
using System.Security.Claims;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Identity;

public class UserClaimsGenerator(IUserInterestRepository interestRepository) : IUserClaimsGenerator
{
    // The constant string for the claim type in the JWT payload
    public const string InterestClaimType = "user_interest"; 

    public async Task<List<Claim>> GenerateUserClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var claims = new List<Claim>();
        var interests = await interestRepository.GetInterestsByUserIdAsync(user.Id, cancellationToken);

        foreach (var interest in interests)
        {
            claims.Add(new Claim(InterestClaimType, interest.ToString()));
        }

        // Note: Other core claims (NameIdentifier, Email) are often added here 
        // OR in the final JwtGenerator, depending on preference.
        // We added them in the JwtGenerator in the last step, so we stick to just the interests here.

        return claims;
    }
}