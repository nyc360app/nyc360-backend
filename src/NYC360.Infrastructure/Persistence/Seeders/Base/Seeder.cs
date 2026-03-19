namespace NYC360.Infrastructure.Persistence.Seeders.Base;

public static class Seeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var seeders = new List<ISeeder>
        {
            new SuperAdminRoleSeeder(),
            new SuperAdminUserSeeder(),
            new ResidentRoleSeeder(),
            new OrganizationRoleSeeder(),
            new VisitorRoleSeeder(),
            new TopicSeeder(),
            new NewsTagSeeder()
        };

        foreach (var seeder in seeders)
            await seeder.SeedAsync(services);
    }
}
