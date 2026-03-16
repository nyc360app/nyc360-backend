using NYC360.Domain.Enums.Communities;
using NYC360.Domain.Dtos.Location;

namespace NYC360.API.Models.Communities;

public record CreateCommunityRequest(
    string Name,
    string Description,
    string? Slug,
    CommunityType Type,
    int? LocationId,
    bool IsPrivate,
    IFormFile? AvatarImage,
    IFormFile? CoverImage
);