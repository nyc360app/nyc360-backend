using NYC360.Application.Features.Authentication.Commands.Register.Visitor;
using NYC360.API.Models.Authentication.Register;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Authentication.Register;

public class VisitorRegisterEndpoint(IMediator mediator) : Endpoint<VisitorRegisterRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/auth/register/visitor");
        AllowAnonymous();
    }

    public override async Task HandleAsync(VisitorRegisterRequest req, CancellationToken ct)
    {
        var command = new VisitorRegisterCommand(
            req.FirstName, 
            req.LastName!, 
            req.Username, 
            req.Email, 
            req.Password, 
            req.CityOfOrigin,
            req.CountryOfOrigin,
            req.VisitPurpose,
            req.LengthOfStay,
            req.Interests,
            req.ReceiveEventAndCultureRecommendations,
            req.EnableLocationBasedSuggestions,
            req.SavePlacesEventsGuides,
            req.DiscoverableProfile,
            req.AllowMessagesFromNycPartners
        );
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}