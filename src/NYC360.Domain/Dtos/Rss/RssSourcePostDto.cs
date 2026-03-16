using NYC360.Domain.Entities;

namespace NYC360.Domain.Dtos.Rss;

public sealed record RssSourcePostDto(int Id, string Name, string? ImageUrl);

public static class RssSourcePostDtoExtension
{
    extension(RssSourcePostDto)
    {
        public static RssSourcePostDto Map(RssFeedSource source)
        {
            return new RssSourcePostDto(source.Id, source.Name!, source.ImageUrl);
        }
    }
}