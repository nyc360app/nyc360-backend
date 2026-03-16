using NYC360.Application.Features.Posts.Commands.SubmitFlag;
using NYC360.API.Models.Post;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Posts;

public class SubmitPostFlagEndpoint(IMediator mediator) : Endpoint<SubmitPostFlagRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/posts/{PostId:int}/report");
    }

    public override async Task HandleAsync(SubmitPostFlagRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var command = new SubmitPostFlagCommand(userId.Value, req.PostId, req.Reason, req.Details);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct); 
    }
}