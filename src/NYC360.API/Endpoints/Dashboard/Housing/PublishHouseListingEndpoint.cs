using NYC360.Application.Features.Housing.Commands.PublishListing;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Dashboard.Housing;

public class PublishHouseListingEndpoint(IMediator mediator) 
    : Endpoint<PublishHouseListingRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/housing-dashboard/publish");
        Permissions(Domain.Constants.Permissions.Housing.Edit);
    }

    public override async Task HandleAsync(PublishHouseListingRequest req, CancellationToken ct)
    {
        var command = new PublishHouseListingCommand(req.HouseId, req.IsPublished);
        var result = await mediator.Send(command, ct);
        
        await Send.OkAsync(result, cancellation: ct);
    }
}
