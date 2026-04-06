using NYC360.Domain.Enums.Communities;

namespace NYC360.API.Models.Communities;

public record CreateCommunityRequest(
    string Name,
    string Description,
    string? Rules,
    string? Slug,
    CommunityType Type,
    string? CategoryCode,
    string? DivisionTag,
    string Borough,
    string Neighborhood,
    string ZipCode,
    int? LocationId,
    bool IsPrivate,
    bool? RequiresApproval,
    bool? AnyoneCanPost,
    IFormFile? AvatarImage,
    IFormFile? CoverImage
);
