using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;
using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Posts;
using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Infrastructure.Persistence.Repositories;
using Xunit;

namespace NYC360.Application.Tests.Infrastructure.Persistence.Repositories;

public class PostRepositoryFeaturedTests
{
    [Fact]
    public async Task GetFeaturedPostsAsync_ReturnsFeaturedFirst_ThenEngagingFallback()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"post-featured-{Guid.NewGuid()}")
            .Options;

        await using var dbContext = new ApplicationDbContext(options);

        dbContext.Posts.AddRange(
            new Post
            {
                Id = 1,
                Title = "Featured Older",
                Content = "Content 1",
                Category = Category.News,
                PostType = PostType.News,
                SourceType = PostSource.User,
                IsApproved = true,
                IsFeatured = true,
                FeaturedAt = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                LastUpdated = DateTime.UtcNow,
                Stats = new PostStats { Likes = 1, Comments = 1, Shares = 1 }
            },
            new Post
            {
                Id = 2,
                Title = "Featured Latest",
                Content = "Content 2",
                Category = Category.News,
                PostType = PostType.News,
                SourceType = PostSource.User,
                IsApproved = true,
                IsFeatured = true,
                FeaturedAt = new DateTime(2026, 4, 2, 10, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2026, 3, 22, 0, 0, 0, DateTimeKind.Utc),
                LastUpdated = DateTime.UtcNow,
                Stats = new PostStats { Likes = 0, Comments = 0, Shares = 0 }
            },
            new Post
            {
                Id = 3,
                Title = "Most Engaging",
                Content = "Content 3",
                Category = Category.Health,
                PostType = PostType.News,
                SourceType = PostSource.User,
                IsApproved = true,
                IsFeatured = false,
                CreatedAt = new DateTime(2026, 3, 30, 0, 0, 0, DateTimeKind.Utc),
                LastUpdated = DateTime.UtcNow,
                Stats = new PostStats { Likes = 20, Comments = 5, Shares = 2 }
            },
            new Post
            {
                Id = 4,
                Title = "Less Engaging",
                Content = "Content 4",
                Category = Category.Culture,
                PostType = PostType.News,
                SourceType = PostSource.User,
                IsApproved = true,
                IsFeatured = false,
                CreatedAt = new DateTime(2026, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                LastUpdated = DateTime.UtcNow,
                Stats = new PostStats { Likes = 2, Comments = 1, Shares = 0 }
            });

        await dbContext.SaveChangesAsync();

        var repository = new PostRepository(dbContext);
        var result = await repository.GetFeaturedPostsAsync(userId: null, count: 3, CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.Equal(2, result[0].Id);
        Assert.Equal(1, result[1].Id);
        Assert.Equal(3, result[2].Id);
        Assert.All(result.Take(2), post => Assert.True(post.IsFeatured));
    }
}
