using Microsoft.AspNetCore.Http;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.UpdateForum;

public record UpdateForumCommand(
    int Id,
    string Title,
    string Slug,
    string Description,
    IFormFile? IconFile,
    bool IsActive
) : IRequest<StandardResponse>;
