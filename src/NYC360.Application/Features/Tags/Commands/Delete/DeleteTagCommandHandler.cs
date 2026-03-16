using MediatR;
using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Tags.Commands.Delete;

public class DeleteTagHandler(
    ITagRepository repo,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteTagCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(DeleteTagCommand request, CancellationToken ct)
    {
        var tag = await repo.GetByIdAsync(request.Id);
        
        if (tag == null)
            return StandardResponse.Failure(new ApiError("tag.notfound", "Tag not found."));

        // Business Rule: Prevent orphaning sub-tags
        if (tag.ChildTags != null && tag.ChildTags.Any())
        {
            return StandardResponse.Failure(new ApiError("tag.haschildren", 
                "Cannot delete this tag because it has sub-tags. Please delete the children first."));
        }

        repo.Delete(tag);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}