using NYC360.Infrastructure.Persistence.Seeders.Base;
using NYC360.Infrastructure.Extensions;
using NYC360.Application.Extensions;
using NYC360.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services
    .AddInfrastructureServices(config)
    .AddApplicationServices(config)
    .AddWebApiServices(config);

var app = builder.Build();

app.UseHttpsRedirection();
app.UsePresentationServices();

using (var scope = app.Services.CreateScope())
{
    await Seeder.SeedAsync(scope.ServiceProvider);
}
//app.UseAutoMigration();

app.UseStaticFiles();

app.Run();