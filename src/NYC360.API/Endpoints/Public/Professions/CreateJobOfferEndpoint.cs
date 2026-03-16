using NYC360.Application.Features.Professions.Commands.Create;
using NYC360.API.Models.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class CreateJobOfferEndpoint(IMediator mediator) : Endpoint<CreateJobOfferRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/professions/offers/create");
    }

    public override async Task HandleAsync(CreateJobOfferRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new CreateJobOfferCommand(
            userId.Value,
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