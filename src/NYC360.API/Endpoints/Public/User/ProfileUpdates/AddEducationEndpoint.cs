using NYC360.Application.Features.Users.Commands.ProfileUpdates.AddEducation;
using NYC360.API.Models.Users.ProfileUpdate;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class AddEducationEndpoint(IMediator mediator) : Endpoint<AddEducationRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/users/profile/educations");
    }

    public override async Task HandleAsync(AddEducationRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new AddEducationCommand(
            userId.Value,
            req.School,
            req.Degree,
            req.FieldOfStudy,
            req.StartDate,
            req.EndDate);

        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}