using NYC360.Application.Features.Users.Commands.Dashboard.Delete;
using NYC360.API.Models.Users;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Users;

public class DeleteUserEndpoint(IMediator mediator) : Endpoint<DeleteUserRequest, StandardResponse>
{
    public override void Configure()
    {
        Delete("/users-dashboard/delete/{UserId:int}");
        Permissions(Domain.Constants.Permissions.Users.Delete);
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        Console.WriteLine($"Deleting UserId: {req.UserId}");
        var command = new DeleteUserCommand(req.UserId);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}