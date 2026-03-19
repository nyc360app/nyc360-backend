using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Services;
using NYC360.Domain.Entities;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.RssSources.Commands.ConnectRequest;

public class RssFeedConnectionRequestCreateCommandHandler(
    IRssFeedConnectionRequestRepository requestRepo,
    IRssSourceRepository rssSourceRepo,
    INewsAuthorizationService newsAuthorizationService)
    : IRequestHandler<RssFeedConnectionRequestCreateCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RssFeedConnectionRequestCreateCommand request, CancellationToken cancellationToken)
    {
        var normalizedUrl = request.Url.Trim();
        var normalizedName = request.Name.Trim();
        var normalizedDescription = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        var normalizedImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim();

        if (request.Category == Category.News)
        {
            var access = await newsAuthorizationService.GetAccessAsync(request.RequesterId, cancellationToken);
            if (access == null || !access.CanConnectRss)
                return StandardResponse.Failure(new ApiError("news.rss_forbidden", "Only Publisher-level News users can connect News RSS feeds."));
        }

        var urlExists = await rssSourceRepo.ExistsAsync(normalizedUrl, cancellationToken);
        if (urlExists)
            return StandardResponse.Failure(new ApiError("rss.url_duplicate", "RSS URL already exists in sources."));

        var hasPendingRequest = await requestRepo.HasPendingRequestAsync(normalizedUrl, request.Category, cancellationToken);
        if (hasPendingRequest)
            return StandardResponse.Failure(new ApiError("rss.request_pending", "A pending connection request already exists for this RSS URL."));
        
        var entity = new RssFeedConnectionRequest
        {
            Url = normalizedUrl,
            Category = request.Category,
            Name = normalizedName,
            Description = normalizedDescription,
            ImageUrl = normalizedImageUrl,
            RequesterId = request.RequesterId,
            CreatedAt = DateTime.UtcNow,
            Status = Domain.Enums.RssConnectionStatus.Pending
        };

        await requestRepo.AddAsync(entity, cancellationToken);
        await requestRepo.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}
