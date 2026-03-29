using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities;
using NYC360.Domain.Enums;
using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Infrastructure.Persistence.Repositories;
using Xunit;

namespace NYC360.Application.Tests.Infrastructure.Persistence.Repositories;

public class RssFeedItemRepositoryTests
{
    [Fact]
    public async Task GetLatestByCategoryAsync_ReturnsSortedLatestItemsByPublishedAt()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"rss-items-{Guid.NewGuid()}")
            .Options;

        await using var dbContext = new ApplicationDbContext(options);

        dbContext.RssFeedItems.AddRange(
            new RssFeedItem
            {
                SourceId = 1,
                Category = Category.Health,
                Link = "https://example.com/old",
                LinkHash = "hash-old",
                Title = "Old",
                PublishedAt = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                FetchedAt = DateTime.UtcNow
            },
            new RssFeedItem
            {
                SourceId = 1,
                Category = Category.Health,
                Link = "https://example.com/newer",
                LinkHash = "hash-newer",
                Title = "Newer",
                PublishedAt = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc),
                FetchedAt = DateTime.UtcNow
            },
            new RssFeedItem
            {
                SourceId = 2,
                Category = Category.Health,
                Link = "https://example.com/latest",
                LinkHash = "hash-latest",
                Title = "Latest",
                PublishedAt = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc),
                FetchedAt = DateTime.UtcNow
            },
            new RssFeedItem
            {
                SourceId = 3,
                Category = Category.News,
                Link = "https://example.com/news-only",
                LinkHash = "hash-news",
                Title = "News",
                PublishedAt = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                FetchedAt = DateTime.UtcNow
            });

        await dbContext.SaveChangesAsync();

        var repository = new RssFeedItemRepository(dbContext);
        var result = await repository.GetLatestByCategoryAsync(Category.Health, 2, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("Latest", result[0].Title);
        Assert.Equal("Newer", result[1].Title);
    }
}
