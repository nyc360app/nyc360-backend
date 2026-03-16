using NYC360.Application.Features.Professions.Commands.UpdateOffer;
using NYC360.API.Models.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class UpdateJobOfferEndpoint(IMediator mediator) : Endpoint<UpdateJobOfferRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/professions/offers/{OfferId}/update");
    }

    public override async Task HandleAsync(UpdateJobOfferRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new UpdateJobOfferCommand(
            userId.Value,
            req.OfferId,
            req.Title,
            req.Description,
            req.Requirements,
            req.Benefits,
            req.Responsibilities,
            req.SalaryMin,
            req.SalaryMax,
            req.WorkArrangement,
            req.EmploymentType,
            req.EmploymentLevel,
            req.Address
        );
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, ct);
    }
}