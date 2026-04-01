using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Features.Posts.Commands.FeatureToggle;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Posts;
using Xunit;

namespace NYC360.Application.Tests.Features.Posts.Commands.FeatureToggle;

public class PostFeatureToggleCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenFeatureEnabled_SetsFeaturedFields()
    {
        var post = new Post
        {
            Id = 42,
            Content = "Post content",
            Category = Category.News,
            PostType = PostType.News,
            SourceType = PostSource.User,
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            LastUpdated = DateTime.UtcNow.AddDays(-2)
        };

        var postRepo = new Mock<IPostRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        postRepo.Setup(x => x.GetByIdAsync(42, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var handler = new PostFeatureToggleCommandHandler(
            postRepo.Object,
            unitOfWork.Object,
            NullLogger<PostFeatureToggleCommandHandler>.Instance);

        var result = await handler.Handle(new PostFeatureToggleCommand(42, true, 9001), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.True(result.Data!.IsFeatured);
        Assert.True(post.IsFeatured);
        Assert.Equal(9001, post.FeaturedByUserId);
        Assert.NotNull(post.FeaturedAt);

        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        postRepo.Verify(x => x.Update(post), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenFeatureDisabled_ClearsFeaturedFields()
    {
        var post = new Post
        {
            Id = 77,
            Content = "Existing post",
            Category = Category.Health,
            PostType = PostType.News,
            SourceType = PostSource.User,
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            LastUpdated = DateTime.UtcNow.AddDays(-2),
            IsFeatured = true,
            FeaturedAt = DateTime.UtcNow.AddDays(-1),
            FeaturedByUserId = 33
        };

        var postRepo = new Mock<IPostRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        postRepo.Setup(x => x.GetByIdAsync(77, It.IsAny<CancellationToken>()))
            .ReturnsAsync(post);

        var handler = new PostFeatureToggleCommandHandler(
            postRepo.Object,
            unitOfWork.Object,
            NullLogger<PostFeatureToggleCommandHandler>.Instance);

        var result = await handler.Handle(new PostFeatureToggleCommand(77, false, 5), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.False(result.Data!.IsFeatured);
        Assert.False(post.IsFeatured);
        Assert.Null(post.FeaturedAt);
        Assert.Null(post.FeaturedByUserId);
    }
}
