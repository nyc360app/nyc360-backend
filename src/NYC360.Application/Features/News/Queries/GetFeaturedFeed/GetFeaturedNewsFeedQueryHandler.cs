using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.News;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.News.Queries.GetFeaturedFeed;

public class GetFeaturedNewsFeedQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetFeaturedNewsFeedQuery, StandardResponse<NewsFeaturedFeedDto>>
{
    public async Task<StandardResponse<NewsFeaturedFeedDto>> Handle(GetFeaturedNewsFeedQuery request, CancellationToken ct)
    {
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
        var page = request.Page <= 0 ? 1 : request.Page;

        DateTime? cursorTime = null;
        int? cursorId = null;

        if (!string.IsNullOrWhiteSpace(request.Cursor))
        {
            if (!TryParseCursor(request.Cursor!, out cursorTime, out cursorId))
            {
                return StandardResponse<NewsFeaturedFeedDto>.Failure(
                    new ApiError("news.featured.invalid_cursor", "Invalid cursor."));
            }
        }

        var take = pageSize + 1;
        var items = await postRepository.GetFeaturedNewsFeedAsync(
            request.UserId,
            pageSize,
            page,
            cursorTime,
            cursorId,
            take,
            ct);

        var hasMore = items.Count > pageSize;
        var trimmed = hasMore ? items.Take(pageSize).ToList() : items;

        var nextCursor = BuildNextCursor(trimmed);

        return StandardResponse<NewsFeaturedFeedDto>.Success(
            new NewsFeaturedFeedDto(trimmed, nextCursor, hasMore));
    }

    private static string? BuildNextCursor(List<PostDto> items)
    {
        if (items.Count == 0)
            return null;

        var last = items[^1];
        var keyTime = last.FeaturedAt ?? last.CreatedAt;
        var token = $"{keyTime.Ticks}|{last.Id}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        return Convert.ToBase64String(bytes);
    }

    private static bool TryParseCursor(string cursor, out DateTime? cursorTime, out int? cursorId)
    {
        cursorTime = null;
        cursorId = null;

        try
        {
            var bytes = Convert.FromBase64String(cursor);
            var text = System.Text.Encoding.UTF8.GetString(bytes);
            var parts = text.Split('|');
            if (parts.Length != 2)
                return false;

            if (!long.TryParse(parts[0], out var ticks))
                return false;

            if (!int.TryParse(parts[1], out var id))
                return false;

            cursorTime = new DateTime(ticks, DateTimeKind.Utc);
            cursorId = id;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
