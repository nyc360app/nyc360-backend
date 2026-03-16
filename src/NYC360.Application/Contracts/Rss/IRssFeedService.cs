using NYC360.Domain.Dtos.Rss;

namespace NYC360.Application.Contracts.Rss;

public interface IRssFeedService
{
    Task<RssSourceDto?> FetchSourceDataAsync(string url, CancellationToken ct);
    Task FetchAllFeedDataAsync(CancellationToken ct);
}