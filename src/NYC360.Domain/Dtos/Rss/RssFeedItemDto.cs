using NYC360.Domain.Entities;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Rss;

public record RssFeedItemDto(
    int Id,
    int SourceId,
    Category Category,
    string Title,
    string Link,
    string? Summary,
    string? ImageUrl,
    DateTime PublishedAt);

public static class RssFeedItemDtoExtensions
{
    extension(RssFeedItemDto)
    {
        public static RssFeedItemDto Map(RssFeedItem item)
        {
            return new RssFeedItemDto(
                item.Id,
                item.SourceId,
                item.Category,
                item.Title,
                item.Link,
                item.Summary,
                item.ImageUrl,
                item.PublishedAt);
        }
    }
}
