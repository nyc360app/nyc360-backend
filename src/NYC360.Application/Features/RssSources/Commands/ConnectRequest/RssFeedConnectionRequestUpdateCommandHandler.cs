using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities;
using NYC360.Domain.Enums;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.RssSources.Commands.ConnectRequest;

public class RssFeedConnectionRequestUpdateCommandHandler(
    IRssFeedConnectionRequestRepository requestRepo,
    IRssSourceRepository rssSourceRepo)
    : IRequestHandler<RssFeedConnectionRequestUpdateCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RssFeedConnectionRequestUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await requestRepo.GetByIdAsync(request.RequestId, cancellationToken);
        if (entity is null)
            return StandardResponse.Failure(new ApiError("rss_request.notfound", "Connection request not found."));

        if (entity.Status != RssConnectionStatus.Pending)
             return StandardResponse.Failure(new ApiError("rss_request.processed", "Request has already been processed."));

        entity.Status = request.Status;
        entity.AdminNote = request.AdminNote;
        entity.ProcessedAt = DateTime.UtcNow;

        if (request.Status == RssConnectionStatus.Approved)
        {
            var urlExists = await rssSourceRepo.ExistsAsync(entity.Url, cancellationToken);
            if (urlExists)
            {
                // If it already exists, decided to just mark as approved but not create duplicate
                // Or could error. Let's error for safety.
                return StandardResponse.Failure(new ApiError("rss.url_duplicate", "RSS URL already exists as a source."));
            }

            var newSource = new RssFeedSource
            {
                Name = entity.Name,
                RssUrl = entity.Url,
                Category = entity.Category,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                IsActive = true,
                LastChecked = DateTime.UtcNow
            };
            
            await rssSourceRepo.AddAsync(newSource, cancellationToken);
        }

        requestRepo.Update(entity);
        
        // Save both changes in one transaction (UnitOfWork handles this implicitly if configured, 
        // but here we call SaveChanges on repositories which usually share context)
        // Since they share the same context injection, SaveChanges on either effectively saves both.
        // But to be explicit and safe if logic changes:
        
        // (Assuming standard EF Core DI behavior where DbContext is scoped)
        await requestRepo.SaveChangesAsync(cancellationToken); 

        return StandardResponse.Success();
    }
}
