using NYC360.Domain.Enums.Communities;

namespace NYC360.API.Models.Communities;

public record UpdateCommunityInfoRequest(
    int CommunityId,
    string? Name,
    string? Description,
    CommunityType? Type,
    int? LocationId,
    bool? IsPrivate,
    bool? RequiresApproval,
    IFormFile? AvatarImage,
    IFormFile? CoverImage
);
