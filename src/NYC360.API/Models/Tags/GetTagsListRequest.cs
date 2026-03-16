using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Wrappers;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.Tags;

public record GetTagsListRequest(
    string? SearchTerm,
    TagType? Type,
    Category? Division
) : PagedRequest;