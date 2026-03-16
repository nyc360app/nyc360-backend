using FastEndpoints;
using MediatR;
using NYC360.API.Models.Forums;
using NYC360.Application.Features.Forums.Commands.CreateForum;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Forums;

public class CreateForumEndpoint(IMediator mediator) : Endpoint<CreateForumRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/forums-dashboard/create");
        Permissions(Domain.Constants.Permissions.Forums.Create);
        AllowFileUploads();
    }

    public override async Task HandleAsync(CreateForumRequest request, CancellationToken ct)
    {
        var command = new CreateForumCommand(
            request.Title,
            request.Slug,
            request.Description,
            request.IconFile
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
