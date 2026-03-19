using NYC360.Application.Contracts.Authentication;
using Microsoft.Extensions.DependencyInjection;
using NYC360.Application.Contracts.Services;
using NYC360.Application.Contracts.Storage;
using NYC360.Application.Contracts.OAuth;
using NYC360.Application.Contracts.Rss;
using NYC360.Application.Contracts.Jwt;
using NYC360.Infrastructure.Identity;
using NYC360.Infrastructure.Services;
using NYC360.Infrastructure.Storage;
using NYC360.Application.Contracts;
using NYC360.Infrastructure.Cache;
using NYC360.Infrastructure.OAuth;
using NYC360.Infrastructure.RSS;
using NYC360.Application.Contracts.Infrastructure;
using NYC360.Infrastructure.Stripe;

namespace NYC360.Infrastructure.Extensions;

public static class ServicesRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUserClaimsGenerator, UserClaimsGenerator>();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IOauthGoogleService, OauthGoogleService>();
        services.AddScoped<IRssFeedService, RssFeedService>();
        services.AddScoped<ICommunityPermissionService, CommunityPermissionService>();
        services.AddScoped<INewsAuthorizationService, NewsAuthorizationService>();
        services.AddScoped<IStripeService, StripeService>();

        services.AddSingleton<ILocalStorageService, LocalStorageService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ISlugService, SlugService>();
        services.AddSingleton<ICachingService, RedisCachingService>();
        
        services.AddHostedService<RssDiscoveryBackgroundService>();
    }
}
