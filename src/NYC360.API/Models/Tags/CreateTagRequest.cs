using NYC360.Domain.Enums;
using NYC360.Domain.Enums.Tags;

namespace NYC360.API.Models.Tags;

public record CreateTagRequest(
    string Name,
    TagType Type,
    Category? Division,
    int? ParentTagId
);