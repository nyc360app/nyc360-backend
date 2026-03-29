using NYC360.Domain.Dtos.Rss;

namespace NYC360.Application.Contracts.Rss;

public interface IRssFetchClient
{
    Task<RssFetchResult?> FetchAsync(string url, CancellationToken ct);
}

public record RssFetchResult(
    RssSourceDto Source,
    List<RssFetchItem> Items);

public record RssFetchItem(
    string? Guid,
    string? Link,
    string? Title,
    string? Summary,
    string? Content,
    string? ImageUrl,
    DateTime? PublishedAt,
    string? RawMetadataJson);
