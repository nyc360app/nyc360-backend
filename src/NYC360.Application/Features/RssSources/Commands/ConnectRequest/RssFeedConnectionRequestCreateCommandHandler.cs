using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.RssSources.Commands.ConnectRequest;

public class RssFeedConnectionRequestCreateCommandHandler(
    IRssFeedConnectionRequestRepository requestRepo,
    IRssSourceRepository rssSourceRepo)
    : IRequestHandler<RssFeedConnectionRequestCreateCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RssFeedConnectionRequestCreateCommand request, CancellationToken cancellationToken)
    {
        var urlExists = await rssSourceRepo.ExistsAsync(request.Url, cancellationToken);
        if (urlExists)
            return StandardResponse.Failure(new ApiError("rss.url_duplicate", "RSS URL already exists in sources."));

        // Check if a pending request already exists for this URL
        // Assuming we might want to prevent duplicate pending requests
        // For now, we'll allow it or rely on unique constraint if any (none specified)
        
        var entity = new RssFeedConnectionRequest
        {
            Url = request.Url,
            Category = request.Category,
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            RequesterId = request.RequesterId,
            CreatedAt = DateTime.UtcNow,
            Status = Domain.Enums.RssConnectionStatus.Pending
        };

        await requestRepo.AddAsync(entity, cancellationToken);
        await requestRepo.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}
