using Microsoft.Extensions.FileProviders;
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
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    await Seeder.SeedAsync(scope.ServiceProvider);
}
//app.UseAutoMigration();

var legacyStaticRoot = Path.Combine(AppContext.BaseDirectory, "wwwroot");
if (!string.IsNullOrWhiteSpace(app.Environment.WebRootPath) &&
    !string.Equals(
        Path.GetFullPath(legacyStaticRoot),
        Path.GetFullPath(app.Environment.WebRootPath),
        StringComparison.OrdinalIgnoreCase) &&
    Directory.Exists(legacyStaticRoot))
{
    // Fallback for files uploaded under older hosting layouts where uploads went under AppContext.BaseDirectory.
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(legacyStaticRoot)
    });
}

app.Run();
