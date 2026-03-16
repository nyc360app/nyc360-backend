using NYC360.Infrastructure.Persistence.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using NYC360.Domain.Entities;
using NYC360.Domain.Entities.User;

namespace NYC360.Infrastructure.Extensions;

public static class IdentityConfiguration
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }
}