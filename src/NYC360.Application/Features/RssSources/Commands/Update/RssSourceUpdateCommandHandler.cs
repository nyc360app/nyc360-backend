using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Commands.Update;

public class RssSourceUpdateCommandHandler(
    IRssSourceRepository repo,
    ILocalStorageService storageService)
    : IRequestHandler<RssSourceUpdateCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RssSourceUpdateCommand req, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(req.Id, ct);
        if (entity is null)
            return StandardResponse.Failure(new ApiError("rss.notfound", "RSS source not found."));

        var normalizedUrl = req.RssUrl.Trim();
        if (!string.Equals(entity.RssUrl, normalizedUrl, StringComparison.OrdinalIgnoreCase))
        {
            var exists = await repo.ExistsAsync(normalizedUrl, ct);
            if (exists)
                return StandardResponse.Failure(new ApiError("rss.url_duplicate", "RSS URL already exists."));
        }

        entity.Name = string.IsNullOrWhiteSpace(req.Name) ? null : req.Name.Trim();
        entity.RssUrl = normalizedUrl;
        entity.Category = req.Category;
        entity.Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim();
        entity.IsActive = req.IsActive;

        if (req.Image != null)
        {
            entity.ImageUrl = await storageService.SaveFileAsync(req.Image,"rss-sources");
        }

        repo.Update(entity);
        await repo.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}
