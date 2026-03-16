using NYC360.Application.Features.Professions.Queries.OfferDetails;
using NYC360.Domain.Dtos.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class GetJobOfferDetailsEndpoint(IMediator mediator) : EndpointWithoutRequest<StandardResponse<JobOfferProfileDto>>
{
    public override void Configure()
    {
        Get("/professions/offers/{OfferId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetId();
        var offerId = Route<int>("OfferId");
        var query = new GetJobOfferDetailsQuery(userId, offerId);
        var result = await mediator.Send(query, ct);

        await Send.OkAsync(result, ct);
    }
}