using NYC360.Domain.Entities.User;
using System.Security.Claims;

namespace NYC360.Application.Contracts.Authentication;

public interface IUserClaimsGenerator
{
    Task<List<Claim>> GenerateUserClaimsAsync(ApplicationUser user, CancellationToken cancellationToken);
}