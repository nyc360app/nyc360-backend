using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Rss;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Entities;
using NYC360.Domain.Enums;
using NYC360.Infrastructure.RSS;
using Xunit;

namespace NYC360.Application.Tests.Infrastructure.RSS;

public class RssFeedServiceTests
{
    [Fact]
    public async Task FetchAllFeedDataAsync_InsertsNewItems_AndSkipsDuplicates()
    {
        var source = new RssFeedSource
        {
            Id = 10,
            Name = "Source A",
            RssUrl = "https://example.com/feed",
            Category = Category.News,
            IsActive = true
        };

        var sourceRepo = new Mock<IRssSourceRepository>();
        var itemRepo = new Mock<IRssFeedItemRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var fetchClient = new Mock<IRssFetchClient>();

        sourceRepo.Setup(x => x.GetAllAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync([source]);

        fetchClient.Setup(x => x.FetchAsync(source.RssUrl!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RssFetchResult(
                new RssSourceDto(null, "Source A", source.RssUrl, source.Category, null, null, true, null, null, null, null),
                [
                    new RssFetchItem("guid-1", "https://example.com/article-1", "Article 1", "S1", null, null, DateTime.UtcNow, null),
                    new RssFetchItem("guid-2", "https://example.com/article-2", "Article 2", "S2", null, null, DateTime.UtcNow, null),
                    new RssFetchItem("guid-3", "https://example.com/article-2", "Article 2 dup", "S2", null, null, DateTime.UtcNow, null)
                ]));

        var existingHash = ComputeHash("https://example.com/article-1");
        itemRepo.Setup(x => x.ExistsBySourceAndLinkHashAsync(source.Id, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int _, string hash, CancellationToken _) => string.Equals(hash, existingHash, StringComparison.OrdinalIgnoreCase));
        itemRepo.Setup(x => x.ExistsBySourceAndGuidAsync(source.Id, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        List<RssFeedItem>? insertedItems = null;
        itemRepo.Setup(x => x.AddRangeAsync(It.IsAny<List<RssFeedItem>>(), It.IsAny<CancellationToken>()))
            .Callback<List<RssFeedItem>, CancellationToken>((items, _) => insertedItems = items)
            .Returns(Task.CompletedTask);

        var service = new RssFeedService(
            sourceRepo.Object,
            itemRepo.Object,
            unitOfWork.Object,
            fetchClient.Object,
            Options.Create(new RssIngestionSettings()),
            NullLogger<RssFeedService>.Instance);

        await service.FetchAllFeedDataAsync(CancellationToken.None);

        Assert.NotNull(insertedItems);
        Assert.Single(insertedItems!);
        Assert.Equal("https://example.com/article-2", insertedItems[0].Link);

        itemRepo.Verify(x => x.AddRangeAsync(It.IsAny<List<RssFeedItem>>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FetchAllFeedDataAsync_UsesSourceCategoryForInsertedItems()
    {
        var source = new RssFeedSource
        {
            Id = 22,
            Name = "Source Final Category",
            RssUrl = "https://example.com/final",
            Category = Category.Culture,
            IsActive = true
        };

        var sourceRepo = new Mock<IRssSourceRepository>();
        var itemRepo = new Mock<IRssFeedItemRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var fetchClient = new Mock<IRssFetchClient>();

        sourceRepo.Setup(x => x.GetAllAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync([source]);

        fetchClient.Setup(x => x.FetchAsync(source.RssUrl!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RssFetchResult(
                new RssSourceDto(null, "Source Final Category", source.RssUrl, source.Category, null, null, true, null, null, null, null),
                [new RssFetchItem("guid-final", "https://example.com/culture-post", "Culture Post", "Summary", null, null, DateTime.UtcNow, null)]));

        itemRepo.Setup(x => x.ExistsBySourceAndLinkHashAsync(source.Id, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        itemRepo.Setup(x => x.ExistsBySourceAndGuidAsync(source.Id, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        List<RssFeedItem>? insertedItems = null;
        itemRepo.Setup(x => x.AddRangeAsync(It.IsAny<List<RssFeedItem>>(), It.IsAny<CancellationToken>()))
            .Callback<List<RssFeedItem>, CancellationToken>((items, _) => insertedItems = items)
            .Returns(Task.CompletedTask);

        var service = new RssFeedService(
            sourceRepo.Object,
            itemRepo.Object,
            unitOfWork.Object,
            fetchClient.Object,
            Options.Create(new RssIngestionSettings()),
            NullLogger<RssFeedService>.Instance);

        await service.FetchAllFeedDataAsync(CancellationToken.None);

        Assert.NotNull(insertedItems);
        Assert.Single(insertedItems!);
        Assert.Equal(Category.Culture, insertedItems[0].Category);
    }

    [Fact]
    public async Task FetchAllFeedDataAsync_WhenOneSourceFails_ContinuesProcessingOthers()
    {
        var failingSource = new RssFeedSource
        {
            Id = 31,
            Name = "Failing Source",
            RssUrl = "https://example.com/failing",
            Category = Category.News,
            IsActive = true
        };

        var healthySource = new RssFeedSource
        {
            Id = 32,
            Name = "Healthy Source",
            RssUrl = "https://example.com/healthy",
            Category = Category.Health,
            IsActive = true
        };

        var sourceRepo = new Mock<IRssSourceRepository>();
        var itemRepo = new Mock<IRssFeedItemRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var fetchClient = new Mock<IRssFetchClient>();

        sourceRepo.Setup(x => x.GetAllAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync([failingSource, healthySource]);

        fetchClient.Setup(x => x.FetchAsync(failingSource.RssUrl!, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("fetch failed"));

        fetchClient.Setup(x => x.FetchAsync(healthySource.RssUrl!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RssFetchResult(
                new RssSourceDto(null, "Healthy Source", healthySource.RssUrl, healthySource.Category, null, null, true, null, null, null, null),
                [new RssFetchItem("guid-ok", "https://example.com/ok-post", "OK", "OK summary", null, null, DateTime.UtcNow, null)]));

        itemRepo.Setup(x => x.ExistsBySourceAndLinkHashAsync(healthySource.Id, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        itemRepo.Setup(x => x.ExistsBySourceAndGuidAsync(healthySource.Id, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var insertedCount = 0;
        itemRepo.Setup(x => x.AddRangeAsync(It.IsAny<List<RssFeedItem>>(), It.IsAny<CancellationToken>()))
            .Callback<List<RssFeedItem>, CancellationToken>((items, _) => insertedCount += items.Count)
            .Returns(Task.CompletedTask);

        var service = new RssFeedService(
            sourceRepo.Object,
            itemRepo.Object,
            unitOfWork.Object,
            fetchClient.Object,
            Options.Create(new RssIngestionSettings { RetryCount = 0 }),
            NullLogger<RssFeedService>.Instance);

        await service.FetchAllFeedDataAsync(CancellationToken.None);

        Assert.NotNull(failingSource.LastError);
        Assert.Null(healthySource.LastError);
        Assert.NotNull(healthySource.LastSuccessAt);
        Assert.Equal(1, insertedCount);

        sourceRepo.Verify(x => x.Update(failingSource), Times.AtLeastOnce);
        sourceRepo.Verify(x => x.Update(healthySource), Times.AtLeastOnce);
    }

    private static string ComputeHash(string value)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        var hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
