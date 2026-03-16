using NYC360.Domain.Entities.Tags;
using NYC360.Domain.Enums.Tags;
using NYC360.Domain.Enums;

namespace NYC360.Domain.Dtos.Tags;

public record TagDto(
    int Id,
    string Name,
    TagType Type,
    Category? Division,
    TagDto? Parent,
    List<TagDto>? Children
);

public static class TagDtoExtensions
{
    public static TagDto Map(this Tag tag, bool includeChildren = false)
    {
        return new TagDto(
            tag.Id, 
            tag.Name, 
            tag.Type, 
            tag.Division, 
            // 1. Map Parent but tell the parent NOT to map its children (prevents infinite loop)
            tag.ParentTag != null 
                ? new TagDto(tag.ParentTag.Id, tag.ParentTag.Name, tag.ParentTag.Type, tag.ParentTag.Division, null, null) 
                : null,
            // 2. Map Children only if explicitly requested, and tell children not to map their sub-children
            includeChildren && tag.ChildTags is { Count: > 0 } 
                ? tag.ChildTags.Select(c => c.Map(includeChildren: false)).ToList()
                : null
        );
    }
}