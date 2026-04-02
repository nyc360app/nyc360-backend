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

        var take = pageSize + 1;

        var cursor = ParseCursor(request.Cursor);
        if (cursor.IsInvalid)
        {
            return StandardResponse<NewsFeaturedFeedDto>.Failure(
                new ApiError("news.featured.invalid_cursor", "Invalid cursor."));
        }

        if (cursor.Type == CursorType.Engaging)
        {
            var engaging = await postRepository.GetEngagingNewsSliceAsync(
                request.UserId,
                cursor.Score,
                cursor.Time,
                cursor.Id,
                take,
                ct);

            var hasMore = engaging.Count > pageSize;
            var trimmed = hasMore ? engaging.Take(pageSize).ToList() : engaging;
            var nextCursor = BuildEngagingCursor(trimmed);

            return StandardResponse<NewsFeaturedFeedDto>.Success(
                new NewsFeaturedFeedDto(trimmed, nextCursor, hasMore));
        }

        var featured = await postRepository.GetFeaturedNewsSliceAsync(
            request.UserId,
            cursor.Time,
            cursor.Id,
            take,
            ct);

        var featuredHasMore = featured.Count > pageSize;
        var featuredTrimmed = featuredHasMore ? featured.Take(pageSize).ToList() : featured;

        if (featuredHasMore)
        {
            var nextCursor = BuildFeaturedCursor(featuredTrimmed);
            return StandardResponse<NewsFeaturedFeedDto>.Success(
                new NewsFeaturedFeedDto(featuredTrimmed, nextCursor, true));
        }

        var remaining = pageSize - featuredTrimmed.Count;
        if (remaining <= 0)
        {
            var nextCursor = BuildFeaturedCursor(featuredTrimmed);
            return StandardResponse<NewsFeaturedFeedDto>.Success(
                new NewsFeaturedFeedDto(featuredTrimmed, nextCursor, false));
        }

        var engagingSlice = await postRepository.GetEngagingNewsSliceAsync(
            request.UserId,
            null,
            null,
            null,
            remaining + 1,
            ct);

        var engagingHasMore = engagingSlice.Count > remaining;
        var engagingTrimmed = engagingHasMore ? engagingSlice.Take(remaining).ToList() : engagingSlice;

        var combined = new List<PostDto>(featuredTrimmed.Count + engagingTrimmed.Count);
        combined.AddRange(featuredTrimmed);
        combined.AddRange(engagingTrimmed);

        var nextEngagingCursor = engagingTrimmed.Count > 0 ? BuildEngagingCursor(engagingTrimmed) : null;
        return StandardResponse<NewsFeaturedFeedDto>.Success(
            new NewsFeaturedFeedDto(combined, nextEngagingCursor, engagingHasMore));
    }

    private static string? BuildFeaturedCursor(List<PostDto> items)
    {
        if (items.Count == 0)
            return null;

        var last = items[^1];
        var keyTime = last.FeaturedAt ?? last.CreatedAt;
        var token = $"F|{keyTime.Ticks}|{last.Id}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        return Convert.ToBase64String(bytes);
    }

    private static string? BuildEngagingCursor(List<PostDto> items)
    {
        if (items.Count == 0)
            return null;

        var last = items[^1];
        var score = ComputeScore(last);
        var token = $"E|{score}|{last.CreatedAt.Ticks}|{last.Id}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        return Convert.ToBase64String(bytes);
    }

    private static int ComputeScore(PostDto post)
    {
        var stats = post.Stats;
        if (stats is null)
            return 0;

        return (stats.Likes * 5) + (stats.Comments * 3) + (stats.Shares * 2);
    }

    private static CursorResult ParseCursor(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor))
            return CursorResult.Empty();

        try
        {
            var bytes = Convert.FromBase64String(cursor!);
            var text = System.Text.Encoding.UTF8.GetString(bytes);
            var parts = text.Split('|');

            if (parts.Length < 3)
                return CursorResult.Invalid();

            var typeToken = parts[0];
            if (typeToken == "F" && parts.Length == 3)
            {
                if (!long.TryParse(parts[1], out var ticks) || !int.TryParse(parts[2], out var id))
                    return CursorResult.Invalid();

                return CursorResult.Featured(new DateTime(ticks, DateTimeKind.Utc), id);
            }

            if (typeToken == "E" && parts.Length == 4)
            {
                if (!int.TryParse(parts[1], out var score))
                    return CursorResult.Invalid();
                if (!long.TryParse(parts[2], out var ticks) || !int.TryParse(parts[3], out var id))
                    return CursorResult.Invalid();

                return CursorResult.Engaging(score, new DateTime(ticks, DateTimeKind.Utc), id);
            }

            return CursorResult.Invalid();
        }
        catch
        {
            return CursorResult.Invalid();
        }
    }

    private enum CursorType
    {
        Featured,
        Engaging
    }

    private readonly record struct CursorResult(
        bool IsInvalid,
        CursorType Type,
        int? Score,
        DateTime? Time,
        int? Id)
    {
        public static CursorResult Empty() => new(false, CursorType.Featured, null, null, null);
        public static CursorResult Invalid() => new(true, CursorType.Featured, null, null, null);
        public static CursorResult Featured(DateTime time, int id) => new(false, CursorType.Featured, null, time, id);
        public static CursorResult Engaging(int score, DateTime time, int id) => new(false, CursorType.Engaging, score, time, id);
    }
}
