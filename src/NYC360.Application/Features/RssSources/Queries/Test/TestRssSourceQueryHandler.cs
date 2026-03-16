using MediatR;
using NYC360.Application.Contracts.Rss;
using NYC360.Domain.Dtos.Rss;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Queries.Test;

public class TestRssSourceQueryHandler(IRssFeedService rssFeedService)
    : IRequestHandler<TestRssSourceQuery, StandardResponse<RssSourceDto>>
{
    public async Task<StandardResponse<RssSourceDto>> Handle(TestRssSourceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await rssFeedService.FetchSourceDataAsync(request.Url, cancellationToken);
            if (data is null)
                return StandardResponse<RssSourceDto>.Failure(new ApiError("rss.url_invalid", "RSS URL is invalid or empty."));

            return StandardResponse<RssSourceDto>.Success(data);
        }
        catch (HttpRequestException ex)
        {
            return StandardResponse<RssSourceDto>.Failure(new ApiError("rss.connection_failed", $"Could not reach the RSS source: {ex.Message}"));
        }
        catch (Exception)
        {
            return StandardResponse<RssSourceDto>.Failure(new ApiError("rss.unknown_error", "An unexpected error occurred while processing the feed."));
        }
    }
}
