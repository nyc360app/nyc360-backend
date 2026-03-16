using NYC360.Application.Features.Professions.Queries.SearchJobs;
using NYC360.Domain.Dtos.Professions;
using NYC360.API.Models.Professions;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class SearchJobsEndpoint(IMediator mediator) : Endpoint<SearchJobsRequest, PagedResponse<JobOfferMinimalDto>>
{
    public override void Configure()
    {
        Get("/professions/offers");
    }

    public override async Task HandleAsync(SearchJobsRequest req, CancellationToken ct)
    {
        var query = new SearchJobsQuery(
            req.Search,
            req.LocationId,
            req.Arrangement,
            req.Type,
            req.Level,
            req.MinSalary,
            req.IsActive,
            req.Page,
            req.PageSize
        );

        var result = await mediator.Send(query, ct);
        await Send.OkAsync(result, ct);
    }
}