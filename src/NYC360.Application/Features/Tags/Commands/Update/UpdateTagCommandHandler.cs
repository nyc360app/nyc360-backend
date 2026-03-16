using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Tags.Commands.Update;

public class UpdateTagHandler(
    ITagRepository repo,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateTagCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(UpdateTagCommand request, CancellationToken ct)
    {
        var tag = await repo.GetByIdAsync(request.Id);
        if (tag == null)
            return StandardResponse.Failure(new ApiError("tag.notfound", "Tag not found."));

        // Normalize ParentTagId (treat 0 as null)
        var parentId = request.ParentTagId is null or 0 ? null : request.ParentTagId;

        // Check duplicate name if name is changing
        if (tag.Name != request.Name && await repo.ExistsAtLevelAsync(request.Name, parentId, ct))
            return StandardResponse.Failure(new ApiError("tag.duplicate", "Another tag with this name already exists at this level."));

        // Update properties
        tag.Name = request.Name;
        tag.Type = request.Type;
        tag.Division = request.Division;
        tag.ParentTagId = parentId;

        // Maintain Division consistency from parent
        if (parentId.HasValue)
        {
            var parent = await repo.GetByIdAsync(parentId.Value);
            if (parent == null)
                return StandardResponse.Failure(new ApiError("tag.parent_not_found", "Parent tag not found."));
            
            tag.Division ??= parent.Division;
        }

        repo.Update(tag);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}