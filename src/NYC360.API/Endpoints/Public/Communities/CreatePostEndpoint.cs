using NYC360.Application.Features.Communities.Commands.CreatePost;
using NYC360.API.Models.Communities;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Communities;

public class CreatePostEndpoint(IMediator mediator) : Endpoint<CreateCommunityPostRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/communities/create-post");
        AllowFileUploads();
    }
    
    public override async Task HandleAsync(CreateCommunityPostRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.OkAsync(StandardResponse<int>.Failure(
                new ApiError("auth.invalidEmail", "Email not found")
            ), ct);
            return;
        }
        
        var command = new CreateCommunityPostCommand(
            userId.Value, 
            request.CommunityId,
            request.Title, 
            request.Content,
            request.Tags,
            request.Attachments
        );
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}