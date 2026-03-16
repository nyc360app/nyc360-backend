using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Enums;

namespace NYC360.API.Models.Tags;

public record UpdateTagRequest(
    string Name,
    TagType Type,
    Category? Division,
    int? ParentTagId
);