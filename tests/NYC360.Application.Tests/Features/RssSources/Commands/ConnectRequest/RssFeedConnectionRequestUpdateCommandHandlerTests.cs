using Moq;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Features.RssSources.Commands.ConnectRequest;
using NYC360.Domain.Entities;
using NYC360.Domain.Enums;
using Xunit;

namespace NYC360.Application.Tests.Features.RssSources.Commands.ConnectRequest;

public class RssFeedConnectionRequestUpdateCommandHandlerTests
{
    [Fact]
    public async Task Handle_ApprovedWithCategoryOverride_UsesOverrideCategoryForSource()
    {
        var requestEntity = new RssFeedConnectionRequest
        {
            Id = 12,
            Url = "https://example.com/feed.xml",
            Category = Category.News,
            Name = "Example Feed",
            Status = RssConnectionStatus.Pending
        };

        var requestRepo = new Mock<IRssFeedConnectionRequestRepository>();
        var sourceRepo = new Mock<IRssSourceRepository>();
        RssFeedSource? createdSource = null;

        requestRepo.Setup(x => x.GetByIdAsync(12, It.IsAny<CancellationToken>()))
            .ReturnsAsync(requestEntity);
        sourceRepo.Setup(x => x.ExistsAsync(requestEntity.Url, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        sourceRepo.Setup(x => x.AddAsync(It.IsAny<RssFeedSource>(), It.IsAny<CancellationToken>()))
            .Callback<RssFeedSource, CancellationToken>((source, _) => createdSource = source)
            .Returns(Task.CompletedTask);
        requestRepo.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new RssFeedConnectionRequestUpdateCommandHandler(requestRepo.Object, sourceRepo.Object);

        var result = await handler.Handle(
            new RssFeedConnectionRequestUpdateCommand(
                RequestId: 12,
                Status: RssConnectionStatus.Approved,
                AdminNote: "Approved with move",
                Category: Category.Culture,
                ProcessedByUserId: 77),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(RssConnectionStatus.Approved, requestEntity.Status);
        Assert.Equal(Category.Culture, requestEntity.FinalCategory);
        Assert.Equal(77, requestEntity.ProcessedByUserId);
        Assert.NotNull(requestEntity.ProcessedAt);

        Assert.NotNull(createdSource);
        Assert.Equal(Category.Culture, createdSource!.Category);
        Assert.True(createdSource.IsActive);

        sourceRepo.Verify(x => x.AddAsync(It.IsAny<RssFeedSource>(), It.IsAny<CancellationToken>()), Times.Once);
        requestRepo.Verify(x => x.Update(requestEntity), Times.Once);
        requestRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ApprovedWithoutOverride_UsesRequestedCategoryForSource()
    {
        var requestEntity = new RssFeedConnectionRequest
        {
            Id = 99,
            Url = "https://example.com/general.rss",
            Category = Category.Health,
            Name = "Health Feed",
            Status = RssConnectionStatus.Pending
        };

        var requestRepo = new Mock<IRssFeedConnectionRequestRepository>();
        var sourceRepo = new Mock<IRssSourceRepository>();
        RssFeedSource? createdSource = null;

        requestRepo.Setup(x => x.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync(requestEntity);
        sourceRepo.Setup(x => x.ExistsAsync(requestEntity.Url, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        sourceRepo.Setup(x => x.AddAsync(It.IsAny<RssFeedSource>(), It.IsAny<CancellationToken>()))
            .Callback<RssFeedSource, CancellationToken>((source, _) => createdSource = source)
            .Returns(Task.CompletedTask);
        requestRepo.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new RssFeedConnectionRequestUpdateCommandHandler(requestRepo.Object, sourceRepo.Object);

        var result = await handler.Handle(
            new RssFeedConnectionRequestUpdateCommand(
                RequestId: 99,
                Status: RssConnectionStatus.Approved,
                AdminNote: "Approved as-is",
                Category: null,
                ProcessedByUserId: 101),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Null(requestEntity.FinalCategory);
        Assert.Equal(101, requestEntity.ProcessedByUserId);

        Assert.NotNull(createdSource);
        Assert.Equal(Category.Health, createdSource!.Category);
        Assert.True(createdSource.IsActive);

        sourceRepo.Verify(x => x.AddAsync(It.IsAny<RssFeedSource>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
