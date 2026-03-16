using Microsoft.EntityFrameworkCore;
using NYC360.Infrastructure.Persistence.DbContexts;

namespace NYC360.API.Extensions;

public static class UseAutoMigrationExtension
{
    public static WebApplication UseAutoMigration(this WebApplication app, bool onlyInDevelopment = true)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<WebApplication>>();

        try
        {
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }

        return app;
    }
}