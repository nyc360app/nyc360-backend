using NYC360.Domain.Dtos.User;

namespace NYC360.Application.Contracts.OAuth;

public interface IOauthGoogleService
{
    Task<OauthUserDto?> GetPayloadAsync(string idToken);
}