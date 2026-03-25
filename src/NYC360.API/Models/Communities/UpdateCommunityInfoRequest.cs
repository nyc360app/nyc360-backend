using NYC360.Domain.Enums.Communities;

namespace NYC360.API.Models.Communities;

public record UpdateCommunityInfoRequest(
    int CommunityId,
    string? Name,
    string? Description,
    string? Rules,
    CommunityType? Type,
    int? LocationId,
    bool? IsPrivate,
    bool? AnyoneCanPost,
    bool? RequiresApproval,
    IFormFile? AvatarImage,
    IFormFile? CoverImage
);
