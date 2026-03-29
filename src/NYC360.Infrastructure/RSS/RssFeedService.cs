using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Rss;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Entities;

namespace NYC360.Infrastructure.RSS;

public sealed class RssFeedService(
    IRssSourceRepository sourceRepository,
    IRssFeedItemRepository feedItemRepository,
    IUnitOfWork unitOfWork,
    IRssFetchClient rssFetchClient,
    IOptions<RssIngestionSettings> settings,
    ILogger<RssFeedService> logger)
    : IRssFeedService
{
    private static readonly ConcurrentDictionary<int, byte> SourceLocks = new();

    private readonly RssIngestionSettings _settings = settings.Value ?? new RssIngestionSettings();

    public async Task<RssSourceDto?> FetchSourceDataAsync(string url, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        try
        {
            var result = await ExecuteWithRetryAsync(
                token => rssFetchClient.FetchAsync(NormalizeUrl(url), token),
                ct);

            return result?.Source;
        }
        catch
        {
            return null;
        }
    }

    public async Task FetchAllFeedDataAsync(CancellationToken ct)
    {
        var sources = await sourceRepository.GetAllAsync(null, ct);
        var activeSources = sources
            .Where(x => x.IsActive && !string.IsNullOrWhiteSpace(x.RssUrl))
            .ToList();

        if (activeSources.Count == 0)
            return;

        logger.LogInformation("Starting RSS ingestion for {Count} active sources", activeSources.Count);

        foreach (var source in activeSources)
        {
            if (!SourceLocks.TryAdd(source.Id, 0))
            {
                logger.LogDebug("Skipping RSS source {SourceId} because it is already being processed", source.Id);
                continue;
            }

            try
            {
                await ProcessSingleSourceAsync(source, ct);
            }
            finally
            {
                SourceLocks.TryRemove(source.Id, out _);
            }
        }
    }

    private async Task ProcessSingleSourceAsync(RssFeedSource source, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        source.LastChecked = now;
        source.LastCheckedAt = now;

        try
        {
            var result = await ExecuteWithRetryAsync(
                token => rssFetchClient.FetchAsync(source.RssUrl!, token),
                ct);

            if (result is null)
            {
                source.LastError = "Feed could not be fetched or parsed.";
                sourceRepository.Update(source);
                await unitOfWork.SaveChangesAsync(ct);
                return;
            }

            var newItems = new List<RssFeedItem>();
            var seenLinkHashes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var seenGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in result.Items)
            {
                var normalizedLink = NormalizeLink(item.Link);
                if (string.IsNullOrWhiteSpace(normalizedLink))
                    continue;

                var linkHash = ComputeLinkHash(normalizedLink);
                if (!seenLinkHashes.Add(linkHash))
                    continue;

                var normalizedGuid = NormalizeGuid(item.Guid);
                if (!string.IsNullOrWhiteSpace(normalizedGuid) && !seenGuids.Add(normalizedGuid))
                    continue;

                if (await feedItemRepository.ExistsBySourceAndLinkHashAsync(source.Id, linkHash, ct))
                    continue;

                if (!string.IsNullOrWhiteSpace(normalizedGuid)
                    && await feedItemRepository.ExistsBySourceAndGuidAsync(source.Id, normalizedGuid, ct))
                    continue;

                var title = Truncate(
                    string.IsNullOrWhiteSpace(item.Title) ? normalizedLink : item.Title.Trim(),
                    500);

                var summary = ResolveSummary(item.Summary, item.Content);
                var publishedAt = item.PublishedAt?.ToUniversalTime() ?? now;

                newItems.Add(new RssFeedItem
                {
                    SourceId = source.Id,
                    Category = source.Category,
                    Guid = normalizedGuid,
                    Link = Truncate(normalizedLink, 2000),
                    LinkHash = linkHash,
                    Title = title,
                    Summary = summary,
                    ImageUrl = Truncate(item.ImageUrl?.Trim(), 2000),
                    PublishedAt = publishedAt,
                    FetchedAt = now,
                    RawMetadataJson = Truncate(item.RawMetadataJson, 4000),
                    IsActive = true,
                    IsDeleted = false
                });
            }

            if (newItems.Count > 0)
            {
                await feedItemRepository.AddRangeAsync(newItems, ct);
            }

            source.LastSuccessAt = DateTime.UtcNow;
            source.LastError = null;
            sourceRepository.Update(source);
            await unitOfWork.SaveChangesAsync(ct);

            logger.LogInformation("{SourceName}: inserted {Count} new RSS items", source.Name, newItems.Count);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            source.LastError = Truncate(ex.Message, 1000);
            sourceRepository.Update(source);
            await unitOfWork.SaveChangesAsync(ct);
            logger.LogError(ex, "Error processing RSS source {SourceId} ({SourceName})", source.Id, source.Name);
        }
    }

    private async Task<T?> ExecuteWithRetryAsync<T>(Func<CancellationToken, Task<T?>> action, CancellationToken ct)
    {
        var retryCount = Math.Max(0, _settings.RetryCount);
        var timeoutSeconds = Math.Max(1, _settings.TimeoutSeconds);
        var baseBackoffMs = Math.Max(100, _settings.RetryBackoffMilliseconds);

        Exception? lastError = null;

        for (var attempt = 0; attempt <= retryCount; attempt++)
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            try
            {
                return await action(timeoutCts.Token);
            }
            catch (OperationCanceledException ex) when (!ct.IsCancellationRequested)
            {
                lastError = new TimeoutException($"RSS fetch timed out after {timeoutSeconds} seconds.", ex);
            }
            catch (Exception ex)
            {
                lastError = ex;
            }

            if (attempt < retryCount)
            {
                var delayMs = baseBackoffMs * (int)Math.Pow(2, attempt);
                await Task.Delay(delayMs, ct);
            }
        }

        throw lastError ?? new InvalidOperationException("RSS operation failed unexpectedly.");
    }

    private static string NormalizeUrl(string url)
    {
        if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return "https://" + url;

        return url;
    }

    private static string? NormalizeLink(string? link)
    {
        if (string.IsNullOrWhiteSpace(link))
            return null;

        var trimmed = link.Trim();
        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out var uri))
            return trimmed;

        var builder = new UriBuilder(uri)
        {
            Fragment = string.Empty
        };

        return builder.Uri.ToString().TrimEnd('/');
    }

    private static string? NormalizeGuid(string? guid)
    {
        return string.IsNullOrWhiteSpace(guid) ? null : guid.Trim();
    }

    private static string ComputeLinkHash(string normalizedLink)
    {
        var bytes = Encoding.UTF8.GetBytes(normalizedLink);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static string? ResolveSummary(string? summary, string? content)
    {
        if (!string.IsNullOrWhiteSpace(summary))
            return Truncate(summary.Trim(), 4000);

        if (!string.IsNullOrWhiteSpace(content))
            return Truncate(content.Trim(), 4000);

        return null;
    }

    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return value.Length <= maxLength ? value : value[..maxLength];
    }
}
