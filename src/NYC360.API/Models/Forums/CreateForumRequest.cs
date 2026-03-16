using Microsoft.AspNetCore.Http;

namespace NYC360.API.Models.Forums;

public record CreateForumRequest(
    string Title,
    string Slug,
    string Description,
    IFormFile? IconFile
);

public record UpdateForumRequest(
    int Id,
    string Title,
    string Slug,
    string Description,
    IFormFile? IconFile,
    bool IsActive
);
