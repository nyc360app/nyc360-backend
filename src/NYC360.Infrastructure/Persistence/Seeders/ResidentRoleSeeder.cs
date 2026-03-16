using NYC360.Infrastructure.Persistence.Seeders.Base;
using Microsoft.Extensions.DependencyInjection;
using NYC360.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Constants;
using System.Security.Claims;

namespace NYC360.Infrastructure.Persistence.Seeders;

public class ResidentRoleSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ResidentRoleSeeder>>();
        
        const string roleName = "Resident";
        const int contentLimit = 500;

        // Ensure role exists
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new ApplicationRole { Name = roleName, ContentLimit = contentLimit };
            await roleManager.CreateAsync(role);
            logger.LogInformation("Created role: {Role}", roleName);
        }
        
        // Assign permissions to role
        List<string> permissions = [
            Permissions.Posts.Create,
            Permissions.Posts.Comment,
            Permissions.Posts.Interact
        ];
        
        foreach (var permission in permissions)
        {
            if (await roleManager.RoleHasClaimAsync(role, Permissions.PermissionClaimType, permission)) 
                continue;
            
            await roleManager.AddClaimAsync(role, new Claim(Permissions.PermissionClaimType, permission));
            logger.LogInformation("Assigned permission {Permission} to role {Role}", permission, roleName);
        }
    }
}