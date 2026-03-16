using Microsoft.AspNetCore.Http;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Communities.Commands.CreatePost;

public record CreateCommunityPostCommand(
    int UserId,
    int CommunityId,
    string Title, 
    string Content,
    List<string>? Tags, 
    List<IFormFile>? Attachments
) : IRequest<StandardResponse<int>>;