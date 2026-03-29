using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CodeHollow.FeedReader;
using NYC360.Application.Contracts.Rss;
using NYC360.Domain.Dtos.Rss;
using NYC360.Infrastructure.Extensions.Types;

namespace NYC360.Infrastructure.RSS;

public class CodeHollowRssFetchClient : IRssFetchClient
{
    private static readonly PropertyInfo? PublishingDateProperty = typeof(FeedItem).GetProperty("PublishingDate");

    public async Task<RssFetchResult?> FetchAsync(string url, CancellationToken ct)
    {
        var normalizedUrl = NormalizeUrl(url);
        var feed = await FeedReader.ReadAsync(normalizedUrl, ct);

        if (feed == null)
            return null;

        var source = RssSourceDto.MapFromFeed(feed);

        var items = feed.Items?
            .Select(MapItem)
            .ToList() ?? [];

        return new RssFetchResult(source, items);
    }

    private static RssFetchItem MapItem(FeedItem item)
    {
        var (summary, content) = ExtractDescriptions(item);

        return new RssFetchItem(
            Guid: string.IsNullOrWhiteSpace(item.Id) ? null : item.Id.Trim(),
            Link: string.IsNullOrWhiteSpace(item.Link) ? null : item.Link.Trim(),
            Title: string.IsNullOrWhiteSpace(item.Title) ? null : item.Title.Trim(),
            Summary: summary,
            Content: content,
            ImageUrl: ExtractImageUrl(item),
            PublishedAt: ResolvePublishedAt(item),
            RawMetadataJson: BuildRawMetadata(item)
        );
    }

    private static string NormalizeUrl(string url)
    {
        if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return "https://" + url;

        return url;
    }

    private static DateTime? ResolvePublishedAt(FeedItem item)
    {
        if (PublishingDateProperty?.GetValue(item) is DateTime publishedAt && publishedAt > DateTime.MinValue)
            return DateTime.SpecifyKind(publishedAt, DateTimeKind.Utc);

        return null;
    }

    private static string? BuildRawMetadata(FeedItem item)
    {
        try
        {
            var payload = new
            {
                item.Author,
                item.Categories,
                item.Id,
                item.Link
            };

            return JsonSerializer.Serialize(payload);
        }
        catch
        {
            return null;
        }
    }

    // Helper: extract image URL from common RSS/Atom layouts
    private static string? ExtractImageUrl(FeedItem item)
    {
        try
        {
            var xml = item.SpecificItem.Element;
            if (xml != null)
            {
                var enclosure = xml.Descendants().FirstOrDefault(e =>
                    e.Name.LocalName.Equals("enclosure", StringComparison.OrdinalIgnoreCase));

                var encUrl = enclosure?.Attribute("url")?.Value;
                if (!string.IsNullOrWhiteSpace(encUrl))
                    return encUrl;

                var mrssNs = "http://search.yahoo.com/mrss/";
                var media = xml.Descendants(XName.Get("content", mrssNs)).FirstOrDefault()
                            ?? xml.Descendants(XName.Get("thumbnail", mrssNs)).FirstOrDefault();

                var mediaUrl = media?.Attribute("url")?.Value ?? media?.Attribute("href")?.Value;
                if (!string.IsNullOrWhiteSpace(mediaUrl))
                    return mediaUrl;

                var image = xml.Descendants().FirstOrDefault(e => e.Name.LocalName == "image");
                var imageUrl = image?.Value
                               ?? image?.Attribute("href")?.Value
                               ?? image?.Attribute("url")?.Value;

                if (!string.IsNullOrWhiteSpace(imageUrl))
                    return imageUrl;
            }

            var html = item.Content ?? item.Description;
            if (!string.IsNullOrEmpty(html))
            {
                var match = Regex.Match(html, "<img[^>]+src\\s*=\\s*[\"'](?<src>[^\"']+)[\"']", RegexOptions.IgnoreCase);
                if (match.Success)
                    return match.Groups["src"].Value;
            }
        }
        catch
        {
            // ignored
        }

        return null;
    }

    private static (string? summary, string? content) ExtractDescriptions(FeedItem item)
    {
        var summary = string.IsNullOrWhiteSpace(item.Description) ? null : item.Description.Trim();
        var content = string.IsNullOrWhiteSpace(item.Content) ? null : item.Content.Trim();

        var xml = item.SpecificItem.Element;
        if (xml == null)
            return (summary, content);

        const string mrssNs = "http://search.yahoo.com/mrss/";
        var mediaDesc = xml.Descendants(XName.Get("description", mrssNs)).FirstOrDefault()?.Value;

        if (string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(mediaDesc))
            content = mediaDesc.Trim();

        return (summary, content);
    }
}
