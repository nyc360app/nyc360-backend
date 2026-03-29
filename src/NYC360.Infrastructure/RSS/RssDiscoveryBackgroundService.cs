using Microsoft.Extensions.DependencyInjection;
using NYC360.Application.Contracts.Rss;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NYC360.Infrastructure.RSS;

public class RssDiscoveryBackgroundService(
    ILogger<RssDiscoveryBackgroundService> logger, 
    IServiceProvider services,
    IOptions<RssIngestionSettings> settings)
    : BackgroundService
{
    private readonly RssIngestionSettings _settings = settings.Value ?? new RssIngestionSettings();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RSS Background Worker Started.");
        var intervalMinutes = Math.Max(1, _settings.IntervalMinutes);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            
            try
            {
                using var scope = services.CreateScope();
                var rssFeedService = scope.ServiceProvider.GetRequiredService<IRssFeedService>();
                await rssFeedService.FetchAllFeedDataAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "💥 Unexpected error in RSS Worker");
            }

            await Task.Delay(TimeSpan.FromMinutes(intervalMinutes), stoppingToken);
        }
        
        logger.LogInformation("RSS Background Worker Finished.");
    }
}
