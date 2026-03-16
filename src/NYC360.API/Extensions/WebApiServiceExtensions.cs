using FastEndpoints.Swagger;
using FastEndpoints;
using NYC360.API.Filters;

namespace NYC360.API.Extensions;

public static class WebApiServiceExtensions
{
    public static void AddWebApiServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureAuth(configuration);
        
        // Add FastEndpoints
        services.AddFastEndpoints();

        // Add Swagger for FastEndpoints
        // This is a FastEndpoints-specific replacement for AddSwaggerGen()
        services.SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "NYC360 API";
                s.Version = "v1";
                //s.SchemaSettings.SchemaProcessors.Clear(); 
                s.SchemaSettings.SchemaProcessors.Add(new EnumSchemaProcessor());
            };
            //o.ShortSchemaNames = true;
        });

        services.ConfigureCors(configuration);
    }

    public static void UsePresentationServices(this WebApplication app)
    {
        app.UseExceptionHandler(builder => { });
        app.UseCors();
        
        // These must be in the correct order: Authentication first, then Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseOpenApi();      // Generates the JSON/YAML
        
        // This maps all your endpoints and replaces app.MapControllers()
        app.UseFastEndpoints(c =>
        {
            c.Endpoints.RoutePrefix = "api";
        })
        .UseSwaggerGen();
    }
}