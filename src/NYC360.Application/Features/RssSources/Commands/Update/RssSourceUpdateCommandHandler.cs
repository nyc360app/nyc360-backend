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

        entity.Name = req.Name;
        entity.RssUrl = req.RssUrl;
        entity.Category = req.Category;
        entity.Description = req.Description;
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