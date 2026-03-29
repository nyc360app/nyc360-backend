using NYC360.Domain.Entities;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Rss;

public record RssSourceDto(
    int? Id,
    string Name,
    string? RssUrl,
    Category? Category,
    string? Description,
    string? ImageUrl,
    bool? IsActive,
    DateTime? LastChecked,
    DateTime? LastCheckedAt,
    DateTime? LastSuccessAt,
    string? LastError
);

public static class RssSourceDtoExtensions
{
    extension(RssSourceDto)
    {
        public static RssSourceDto Map(RssFeedSource source)
        {
            return new RssSourceDto(
                source.Id,
                source.Name!,
                source.RssUrl!,
                source.Category,
                source.Description,
                source.ImageUrl,
                source.IsActive,
                source.LastChecked,
                source.LastCheckedAt,
                source.LastSuccessAt,
                source.LastError
            );
        }
    }
}
