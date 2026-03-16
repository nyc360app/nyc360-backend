using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.Post;
using NYC360.Application.Features.Posts.Commands.Create;
using NYC360.Domain.Dtos.Posts;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Posts;

public class CreatePostEndpoint(IMediator mediator) : Endpoint<CreatePostRequest, StandardResponse<PostDto>>
{
    public override void Configure()
    {
        Post("/posts-dashboard/create");
        Permissions(Domain.Constants.Permissions.Posts.Create);
        AllowFileUploads();
    }
    
    public override async Task HandleAsync(CreatePostRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<PostDto>.Failure(
                new ApiError("auth.invalidEmail", "Email not found")
            ), ct);
            return;
        }
        
        var command = new PostCreateCommand(
            userId.Value, 
            request.Title, 
            request.Content, 
            request.Category,
            request.TopicId,
            request.Type,
            request.LocationId,
            request.Tags,
            request.Attachments
        );
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}