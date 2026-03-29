using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NYC360.Infrastructure.RSS;

namespace NYC360.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RssIngestionSettings>(configuration.GetSection("RssIngestion"));
        services.ConfigureDatabase(configuration);
        services.ConfigureIdentity();
        services.ConfigureCache(configuration);
        services.RegisterRepositories();
        services.RegisterEmailServices(configuration);
        services.RegisterServices();
        return services;
    }
}
