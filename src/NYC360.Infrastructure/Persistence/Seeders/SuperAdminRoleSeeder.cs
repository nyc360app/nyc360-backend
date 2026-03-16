using NYC360.Infrastructure.Persistence.Seeders.Base;
using Microsoft.Extensions.DependencyInjection;
using NYC360.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NYC360.Domain.Constants;
using NYC360.Domain.Entities;
using System.Security.Claims;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Persistence.Seeders;

public class SuperAdminRoleSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SuperAdminRoleSeeder>>();
        
        const string roleName = "SuperAdmin";
        const int contentLimit = 50000;

        // Ensure SuperAdmin role exists
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new ApplicationRole { Name = roleName, ContentLimit = contentLimit };
            await roleManager.CreateAsync(role);
            logger.LogInformation("Created role: {Role}", roleName);
        }

        // Assign all permissions to SuperAdmin role
        var allPermissions = Permissions.GetAllPermissions();
        foreach (var permission in allPermissions)
        {
            if (await roleManager.RoleHasClaimAsync(role, Permissions.PermissionClaimType, permission))
                continue;
            await roleManager.AddClaimAsync(role, new Claim(Permissions.PermissionClaimType, permission));
            logger.LogInformation("Assigned permission {Permission} to role {Role}", permission, roleName);
        }
    }
}