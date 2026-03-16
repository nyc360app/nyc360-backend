using NYC360.Application.Features.Users.Commands.ProfileUpdates.UpdateEducation;
using NYC360.API.Models.Users.ProfileUpdate;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.User.ProfileUpdates;

public class UpdateEducationEndpoint(IMediator mediator) : Endpoint<UpdateEducationRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/users/profile/educations");
    }

    public override async Task HandleAsync(UpdateEducationRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new UpdateEducationCommand(
            userId.Value, req.EducationId, req.School, req.Degree, req.FieldOfStudy, req.StartDate, req.EndDate), ct);
        
        await Send.OkAsync(result, ct);
    }
}