using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Dtos.Tags;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;
using MediatR;

namespace NYC360.Application.Features.Tags.Queries.List;

public record GetTagsPagedListQuery(
    string? SearchTerm,
    TagType? Type,
    Category? Division,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedResponse<TagDto>>;