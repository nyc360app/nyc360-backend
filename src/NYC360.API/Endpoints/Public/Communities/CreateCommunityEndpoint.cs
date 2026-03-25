using NYC360.Application.Features.Communities.Commands.Create;
using NYC360.API.Models.Communities;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.Communities;

public class CreateCommunityEndpoint(IMediator mediator) : Endpoint<CreateCommunityRequest, StandardResponse<string>>
{
    public override void Configure()
    {
        Post("/communities/create");
        Roles("Resident", "Organization", "Business", "Admin", "SuperAdmin");
        AllowFileUploads();
    }
    
    public override async Task HandleAsync(CreateCommunityRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        var command = new CreateCommunityCommand(
            userId.Value,
            req.Name,
            req.Description,
            req.Rules,
            req.Slug,
            req.Type,
            req.LocationId,
            req.IsPrivate,
            req.RequiresApproval,
            req.AnyoneCanPost,
            req.AvatarImage,
            req.CoverImage
        );
        
        var result = await mediator.Send(command, ct);

        await Send.OkAsync(result, ct);
    }
}
