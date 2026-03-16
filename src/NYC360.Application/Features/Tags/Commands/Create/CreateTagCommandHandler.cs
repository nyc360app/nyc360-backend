using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Tags.Commands.Create;

public class CreateTagHandler(
    ITagRepository repo,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateTagCommand, StandardResponse>
{
    public async Task<StandardResponse> Handle(CreateTagCommand request, CancellationToken ct)
    {
        // Normalize ParentTagId (treat 0 as null)
        var parentId = request.ParentTagId is null or 0 ? null : request.ParentTagId;

        // 1. Check for duplicates at the same hierarchical level
        if (await repo.ExistsAtLevelAsync(request.Name, parentId, ct))
            return StandardResponse.Failure(new ApiError("tag.duplicate", "Tag already exists."));

        // 2. Map Command to Entity
        var tag = new Tag
        {
            Name = request.Name,
            Type = request.Type,
            Division = request.Division,
            ParentTagId = parentId
        };

        // 3. Logic: Inherit Division from Parent if child is professional/interest
        if (parentId.HasValue)
        {
            var parent = await repo.GetByIdAsync(parentId.Value);
            if (parent == null)
                return StandardResponse.Failure(new ApiError("tag.parent_not_found", "Parent tag not found."));

            tag.Division ??= parent.Division;
        }

        await repo.AddAsync(tag);
        await unitOfWork.SaveChangesAsync(ct);

        return StandardResponse.Success();
    }
}