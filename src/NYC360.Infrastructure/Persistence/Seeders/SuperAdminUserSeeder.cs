using NYC360.Infrastructure.Persistence.Seeders.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NYC360.Domain.Entities.User;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Users;

namespace NYC360.Infrastructure.Persistence.Seeders;

public class SuperAdminUserSeeder : ISeeder
{
    public async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SuperAdminUserSeeder>>();

        const string superAdminEmail = "superadmin@nyc360.com";
        const string superAdminPassword = "superadmin@nyc360";
        const string superAdminRole = "SuperAdmin";
        
        // Ensure SuperAdmin user exists
        var user = await userManager.FindByEmailAsync(superAdminEmail);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "nyc360-superadmin",
                Email = superAdminEmail,
                EmailConfirmed = true,
                Type = UserType.Admin,
                Profile = new UserProfile
                {
                    FirstName = "SuperAdmin" ,
                    LastName = "NYC360"
                }
            };

            var result = await userManager.CreateAsync(user, superAdminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, superAdminRole);
                logger.LogInformation("Seeded SuperAdmin user.");
            }
            else
            {
                logger.LogError("Failed to create SuperAdmin user: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else if (!await userManager.IsInRoleAsync(user, superAdminRole))
        {
            await userManager.AddToRoleAsync(user, superAdminRole);
            logger.LogInformation("Assigned SuperAdmin role to existing user.");
        }
    }
}