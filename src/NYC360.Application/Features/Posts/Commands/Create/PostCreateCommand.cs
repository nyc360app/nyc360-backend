using Microsoft.AspNetCore.Http;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Enums;
using MediatR;
using NYC360.Domain.Enums.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.Application.Features.Posts.Commands.Create;

public record PostCreateCommand(
    int UserId, 
    string Title, 
    string Content, 
    Category Category, 
    int? TopicId,
    PostType Type,
    int? LocationId,
    List<int>? Tags,
    List<IFormFile>? Attachments
) : IRequest<StandardResponse<PostDto>>;