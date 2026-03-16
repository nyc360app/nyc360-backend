using NYC360.Infrastructure.Identity;
using System.Security.Claims;
using NYC360.Domain.Enums;

namespace NYC360.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal user)
    {
        public int? GetId()
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userId, out var id) ? id : null;
        }

        public string? GetRole()
        {
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }

        public bool Is(int id) => id == user.GetId();
        
        public List<Category> GetInterests()
        {
            var interests = new List<Category>();
            var interestClaims = user.FindAll(UserClaimsGenerator.InterestClaimType);

            foreach (var claim in interestClaims)
            {
                if (Enum.TryParse<Category>(claim.Value, ignoreCase: true, out var category))
                {
                    interests.Add(category);
                }
            }

            return interests;
        }
    }
}