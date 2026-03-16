using NYC360.Application.Features.Authentication.Commands.Register.Organization;
using NYC360.API.Models.Authentication.Register;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Authentication.Register;

public class OrganizationRegisterEndpoint(IMediator mediator) : Endpoint<OrganizationRegisterRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/auth/register/organization");
        AllowAnonymous();
    }

    public override async Task HandleAsync(OrganizationRegisterRequest req, CancellationToken ct)
    {
        var command = new RegisterOrganizationCommand(
            req.Name,
            req.Username,
            req.Email,
            req.Password,
            req.OrganizationType,
            req.ServiceArea,
            req.Services,
            req.Website,
            req.PhoneNumber,
            req.PublicEmail,
            req.Description,
            req.SocialLinks,
            req.Interests,
            req.Address,
            req.FundType,
            req.IsTaxExempt,
            req.IsNysRegistered
        );
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}