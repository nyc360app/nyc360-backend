using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Tags.Queries.GetById;

public class GetTagByIdQueryHandler(ITagRepository repo) 
    : IRequestHandler<GetTagByIdQuery, StandardResponse<TagDto>>
{
    public async Task<StandardResponse<TagDto>> Handle(GetTagByIdQuery request, CancellationToken ct)
    {
        var tag = await repo.GetByIdAsync(request.Id);
        
        if (tag == null)
        {
            return StandardResponse<TagDto>.Failure(new ApiError("tag.notFound", "Tag not found."));
        }

        return StandardResponse<TagDto>.Success(tag.Map(includeChildren: true));
    }
}
