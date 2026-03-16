using NYC360.Application.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;
using NYC360.Infrastructure.Extensions.Types;
using NYC360.Application.Contracts.Rss;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Entities;
using NYC360.Domain.Dtos.Rss;
using CodeHollow.FeedReader;
using System.Xml.Linq;

namespace NYC360.Infrastructure.RSS;

public sealed class RssFeedService(
    IRssSourceRepository sourceRepository,
    IPostRepository postRepository,
    IServiceScopeFactory scopeFactory,
    ILogger<RssFeedService> logger) 
    : IRssFeedService
{
    public async Task<RssSourceDto?> FetchSourceDataAsync(string url, CancellationToken ct)
    {
        if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            url = "https://" + url;
        
        var feed = await FeedReader.ReadAsync(url, ct);

        return feed == null ? null : RssSourceDto.MapFromFeed(feed);
    }

   public async Task FetchAllFeedDataAsync(CancellationToken ct)
    {
        var sources = await sourceRepository.GetAllAsync(ct);
        if (sources.Count == 0) return;

        logger.LogInformation("📝 Starting Scoped Parallel RSS Fetch for {Count} sources", sources.Count);

        var tasks = sources.Select(source => Task.Run(async () =>
        {
            using var scope = scopeFactory.CreateScope();
            
            var localPostRepo = scope.ServiceProvider.GetRequiredService<IPostRepository>();
            var localSourceRepo = scope.ServiceProvider.GetRequiredService<IRssSourceRepository>();
            var localUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await ProcessSingleSourceAsync(source, localPostRepo, localSourceRepo, localUnitOfWork, ct);
        }, ct));

        await Task.WhenAll(tasks);
    }

    private async Task ProcessSingleSourceAsync(
        RssFeedSource source, 
        IPostRepository localPostRepo, 
        IRssSourceRepository localSourceRepo,
        IUnitOfWork localUnitOfWork,
        CancellationToken ct)
    {
        try 
        {
            var items = await FetchFeedItemsAsync(source, ct);
            if (items == null || !items.Any()) return;

            // 1. Filter existing links
            var itemLinks = items.Select(i => i.Link).Where(l => !string.IsNullOrEmpty(l)).ToList();
            var existingLinks = await localPostRepo.GetExistingLinksAsync(itemLinks, ct);
            
            var newItems = items.Where(i => !existingLinks.Contains(i.Link)).ToList();
            if (!newItems.Any()) return;

            // 2. Resolve Tags with Deduplication
            // var allTagNames = newItems
            //     .SelectMany(i => ExtractTags(i))
            //     .Distinct()
            //     .ToList();

            //var tagEntities = await localPostRepo.EnsureTagsExistAsync(allTagNames, ct);
            
            // GroupBy handles cases where DB might have returned two similar tags 
            // or normalization caused a collision.
            // var tagMap = tagEntities
            //     .GroupBy(t => t.Name)
            //     .ToDictionary(g => g.Key, g => g.First());

            // 3. Map items to Posts
            var postsToInsert = new List<Post>();
            foreach (var item in newItems)
            {
                var post = MapFeedItemToPost(item, source);
                
                // Normalize tags for this specific item to match Dictionary keys
                // var itemTags = ExtractTags(item)
                //     .Select(t => t.Trim().ToLower().Replace(" ", "-"))
                //     .Where(t => !string.IsNullOrEmpty(t))
                //     .Distinct();
                //
                // foreach (var tagName in itemTags)
                // {
                //     if (tagMap.TryGetValue(tagName, out var tagEntity))
                //     {
                //         post.Tags.Add(tagEntity);
                //     }
                // }
                postsToInsert.Add(post);
            }

            // 4. Save Changes
            await localPostRepo.AddAsync(postsToInsert, ct);
            
            source.LastChecked = DateTime.UtcNow;
            localSourceRepo.Update(source); 
            
            await localUnitOfWork.SaveChangesAsync(ct);
            
            logger.LogInformation("✅ {SourceName}: Imported {Count} posts.", source.Name, postsToInsert.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Error processing RSS source: {SourceName}", source.Name);
        }
    }
    private async Task<List<FeedItem>?> FetchFeedItemsAsync(RssFeedSource source, CancellationToken ct)
    {
        var feed = await FeedReader.ReadAsync(source.RssUrl, ct);
        if (feed?.Items == null || !feed.Items.Any())
        {
            logger.LogWarning("⚠️ No items found at {FeedUrl}", source.RssUrl);
            return null;
        }
        
        // TODO: Filter out already existing items in cache (when cache is implemented)

        return feed.Items.ToList();
    }
    
    private Post MapFeedItemToPost(FeedItem item, RssFeedSource source)
    {
        var imageUrl = ExtractImageUrl(item);
        var (summary, content) = ExtractDescriptions(item);
        var finalContent = summary;

        if (!string.IsNullOrWhiteSpace(content))
        {
            if (!string.IsNullOrWhiteSpace(summary)) finalContent += "\n\n";
            finalContent += content;
        }

        if (!string.IsNullOrEmpty(item.Author))
            finalContent += $"\n\nWritten by: {item.Author}";

        if (!string.IsNullOrWhiteSpace(item.Link))
            finalContent += $"\n\nSee more at: {item.Link}";

        var entity = new Post
        {
            SourceType = PostSource.Rss,
            PostType = PostType.News,
            Category = source.Category,
            Title = item.Title,
            Content = finalContent,
            IsApproved = true,
            SourceId = source.Id,
            CreatedAt = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            entity.Attachments.Add(new PostAttachment { Url = imageUrl });
        }

        return entity;
    }
    
    // 🧩 Helper: Extract image URL from multiple possible RSS/Atom formats
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
                var m = Regex.Match(html, "<img[^>]+src\\s*=\\s*[\"'](?<src>[^\"']+)[\"']", RegexOptions.IgnoreCase);
                if (m.Success)
                    return m.Groups["src"].Value;
            }
        }
        catch
        {
            // ignored
        }

        return null;
    }
    
    private List<string> ExtractTags(FeedItem item)
    {
        var tags = new List<string>();
    
        // 1. Standard RSS Categories
        if (item.Categories != null)
            tags.AddRange(item.Categories);

        // 2. Dubin Core Subjects (Alternative RSS format)
        var dcNamespace = "http://purl.org/dc/elements/1.1/";
        var subjects = item.SpecificItem.Element?
            .Descendants(XName.Get("subject", dcNamespace))
            .Select(x => x.Value);
    
        if (subjects != null)
            tags.AddRange(subjects);

        return tags.Distinct().ToList();
    }
    
    // Extract Summary + Content with fallback to <media:description>
    private static (string? summary, string? content) ExtractDescriptions(FeedItem item)
    {
        var summary = item.Description;
        var content = item.Content;

        var xml = item.SpecificItem.Element;
        if (xml == null)
            return (summary, content);

        const string mrssNs = "http://search.yahoo.com/mrss/";

        var mediaDesc =
            xml.Descendants(XName.Get("description", mrssNs)).FirstOrDefault()?.Value;

        // Only use <media:description> if there is no <content>
        if (string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(mediaDesc))
            content = mediaDesc.Trim();

        return (summary, content);
    }
}