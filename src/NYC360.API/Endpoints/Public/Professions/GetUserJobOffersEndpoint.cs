using NYC360.Application.Features.Professions.Queries.UserOffers;
using NYC360.Domain.Dtos.Professions;
using NYC360.API.Models.Professions;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Professions;

public class GetUserJobOffersEndpoint(IMediator mediator) : Endpoint<GetUserOffersListRequest, PagedResponse<JobOfferManageDto>>
{
    public override void Configure()
    {
        Get("/professions/offers/my-offers");
    }

    public override async Task HandleAsync(GetUserOffersListRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var query = new GetUserJobOffersQuery(userId.Value, request.IsActive, request.Page, request.PageSize);
        var result = await mediator.Send(query, ct);
        
        await Send.OkAsync(result, ct);
    }
}