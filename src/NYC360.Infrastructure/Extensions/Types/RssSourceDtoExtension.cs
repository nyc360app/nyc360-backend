using CodeHollow.FeedReader;
using NYC360.Domain.Dtos.Rss;

namespace NYC360.Infrastructure.Extensions.Types;

public static class RssSourceDtoExtension
{
    extension(RssSourceDto)
    {
        public static RssSourceDto MapFromFeed(Feed feed)
        {
            return new RssSourceDto(
                null,
                feed.Title,
                null,
                null,
                feed.Description,
                feed.ImageUrl,
                null,
                null,
                null,
                null,
                null
            );
        }
    }
}
