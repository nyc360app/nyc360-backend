using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace NYC360.Infrastructure.Extensions;

public static class CacheConfiguration
{
    public static void ConfigureCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("CacheConnection");
        });
    }
}