using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities;
using MediatR;
using NYC360.Domain.Wrappers;
using NYC360.Application.Contracts.Storage;

namespace NYC360.Application.Features.RssSources.Commands.Create;

public class RssSourceCreateCommandHandler(
    IRssSourceRepository rssSourceRepo,
    ILocalStorageService storageService)
    : IRequestHandler<RssSourceCreateCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RssSourceCreateCommand request, CancellationToken cancellationToken)
    {
        var normalizedUrl = request.Url.Trim();
        var normalizedName = request.Name.Trim();
        var normalizedDescription = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        var normalizedImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim();

        var urlExists = await rssSourceRepo.ExistsAsync(normalizedUrl, cancellationToken);
        if (urlExists)
            return StandardResponse.Failure(new ApiError("rss.url_duplicate", "RSS URL already exists."));

        string? imageUrl = normalizedImageUrl;
        if (request.Image is not null)
        {
            imageUrl = await storageService.SaveFileAsync(request.Image, "rss-feeds", cancellationToken);
        }

        var entity = new RssFeedSource
        {
            Name = normalizedName,
            RssUrl = normalizedUrl,
            Category = request.Category,
            Description = normalizedDescription,
            ImageUrl = imageUrl,
            IsActive = true,
            LastChecked = DateTime.UtcNow,
            LastCheckedAt = DateTime.UtcNow
        };

        await rssSourceRepo.AddAsync(entity, cancellationToken);
        await rssSourceRepo.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}
