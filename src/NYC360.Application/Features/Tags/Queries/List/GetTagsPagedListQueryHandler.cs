using NYC360.Application.Contracts.Persistence;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Tags.Queries.List;

public class GetTagsPagedListQueryHandler(ITagRepository repo) 
    : IRequestHandler<GetTagsPagedListQuery, PagedResponse<TagDto>>
{
    public async Task<PagedResponse<TagDto>> Handle(GetTagsPagedListQuery request, CancellationToken ct)
    {
        var (items, total) = await repo.GetPagedTagsAsync(
            request.SearchTerm, 
            request.Type, 
            request.Division, 
            request.PageNumber, 
            request.PageSize, 
            ct);

        var dtos = items.Select(t => t.Map(includeChildren: false)).ToList();

        return PagedResponse<TagDto>.Create(dtos, request.PageNumber, request.PageSize, total);
    }
}