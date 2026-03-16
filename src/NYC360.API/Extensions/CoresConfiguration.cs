namespace NYC360.API.Extensions;

public static class CoresConfiguration
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();

            if (allowedOrigins == null || allowedOrigins.Length == 0)
            {
                throw new InvalidOperationException("CORS AllowedOrigins is not configured in appsettings.json");
            }

            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(allowedOrigins) // Use the origins from your config file
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials(); // CRITICAL: This is required for SignalR with authentication
            });
        });
        
        return services;
    }
}