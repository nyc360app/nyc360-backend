using NYC360.Application.Contracts.Persistence;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.RssSources.Commands.Delete;

public class RssSourceDeleteCommandHandler(
    IRssSourceRepository rssSourceRepository)
    : IRequestHandler<RssSourceDeleteCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(RssSourceDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await rssSourceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return StandardResponse.Failure(new ApiError("rss.notfound", "RSS source not found."));
        
        // TODO: delete all related posts
        
        rssSourceRepository.Remove(entity);
        await rssSourceRepository.SaveChangesAsync(cancellationToken);

        return StandardResponse.Success();
    }
}