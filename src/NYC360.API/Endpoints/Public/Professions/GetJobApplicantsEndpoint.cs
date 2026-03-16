using NYC360.Application.Features.Professions.Queries.GetJobApplicants;
using NYC360.Domain.Dtos.Professions;
using NYC360.API.Models.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class GetJobApplicantsEndpoint(IMediator mediator) 
    : Endpoint<GetJobApplicantsRequest, PagedResponse<JobApplicationDetailsDto>>
{
    public override void Configure()
    {
        Get("/professions/offers/{OfferId}/applicants");
    }

    public override async Task HandleAsync(GetJobApplicantsRequest req, CancellationToken ct)
    {
        var userId = User.GetId();

        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetJobApplicantsQuery(
            req.OfferId,
            userId.Value,
            req.Page,
            req.PageSize
        );
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}