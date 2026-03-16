using Microsoft.AspNetCore.Authentication.JwtBearer;
using FastEndpoints.Security;

namespace NYC360.API.Extensions;

public static class AuthenticationAndAuthorizationConfiguration
{
    public static void ConfigureAuth(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationJwtBearer(s =>
        {
            s.SigningKey = configuration["Jwt:Key"];
        });
        services.AddAuthentication(o =>
        {
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        });
        services.AddAuthorization();
    }
}