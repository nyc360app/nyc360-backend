using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Rss;
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
        var urlExists = await rssSourceRepo.ExistsAsync(request.Url, cancellationToken);
        if (urlExists)
            return StandardResponse.Failure(new ApiError("rss.url_duplicate", "RSS URL already exists."));

        string? imageUrl = request.ImageUrl;
        if (request.Image is not null)
        {
            imageUrl = await storageService.SaveFileAsync(request.Image, "rss-feeds", cancellationToken);
        }

        var entity = new RssFeedSource
        {
            Name = request.Name,
            RssUrl = request.Url,
            Category = request.Category,
            Description = request.Description,
            ImageUrl = imageUrl,
            IsActive = true
        };

        await rssSourceRepo.AddAsync(entity, cancellationToken);
        await rssSourceRepo.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}