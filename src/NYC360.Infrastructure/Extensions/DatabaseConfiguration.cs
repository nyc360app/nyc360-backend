using NYC360.Infrastructure.Persistence.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Extensions;

public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}