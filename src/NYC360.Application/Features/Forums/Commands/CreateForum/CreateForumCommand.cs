using Microsoft.AspNetCore.Http;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Forums.Commands.CreateForum;

public record CreateForumCommand(
    string Title,
    string Slug,
    string Description,
    IFormFile? IconFile
) : IRequest<StandardResponse<int>>;
