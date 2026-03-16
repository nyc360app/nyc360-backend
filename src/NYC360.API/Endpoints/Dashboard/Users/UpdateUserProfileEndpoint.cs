using NYC360.Application.Features.Users.Commands.Dashboard.EditProfile;
using NYC360.API.Models.Users;
using FastEndpoints;
using MediatR;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Dashboard.Users;

public class EditUser(IMediator mediator) : Endpoint<EditUserRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/users-dashboard/update/profile");
        Permissions(Domain.Constants.Permissions.Users.Edit);
        AllowFileUploads();
    }

    public override async Task HandleAsync(EditUserRequest req, CancellationToken ct)
    {
        var command = new DashboardEditUserProfileCommand(req.Id, req.FirstName, req.LastName, req.Email, req.Bio, req.Avatar);
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}