using NYC360.Application.Features.Professions.Commands.Share;
using NYC360.API.Models.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class ShareJobOfferEndpoint(IMediator mediator) 
    : Endpoint<ShareJobOfferRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/professions/offers/{OfferId}/share");
    }

    public override async Task HandleAsync(ShareJobOfferRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new ShareJobOfferCommand(
            userId.Value,
            req.OfferId,
            req.Content,
            req.Tags,
            req.CommunityId
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}