using Microsoft.Extensions.Configuration;
using NYC360.Application.Contracts.OAuth;
using Google.Apis.Auth;
using NYC360.Domain.Dtos.User;

namespace NYC360.Infrastructure.OAuth;

public class OauthGoogleService(
    IConfiguration configuration) 
    : IOauthGoogleService
{
    public async Task<OauthUserDto?> GetPayloadAsync(string idToken)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [configuration["OAuth:Google:ClientId"]]
            });
            Console.WriteLine(payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
        
        return new OauthUserDto(payload.Email, payload.Name, payload.GivenName, payload.FamilyName);
    }
}