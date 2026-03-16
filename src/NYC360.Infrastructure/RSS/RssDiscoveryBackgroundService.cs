using Microsoft.Extensions.DependencyInjection;
using NYC360.Application.Contracts.Rss;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NYC360.Infrastructure.RSS;

public class RssDiscoveryBackgroundService(
    ILogger<RssDiscoveryBackgroundService> logger, 
    IServiceProvider services)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RSS Background Worker Started.");
        
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
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
        
        logger.LogInformation("RSS Background Worker Finished.");
    }
}