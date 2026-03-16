namespace NYC360.Infrastructure.Persistence.Seeders.Base;

public interface ISeeder
{
    Task SeedAsync(IServiceProvider services);
}