using NYC360.Application.Features.Authentication.Commands.Register.Business;
using NYC360.API.Models.Authentication.Register;
using NYC360.Domain.Wrappers;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Authentication.Register;

public class BusinessRegisterEndpoint(IMediator mediator) : Endpoint<BusinessRegisterRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/auth/register/business");
        AllowAnonymous();
    }

    public override async Task HandleAsync(BusinessRegisterRequest req, CancellationToken ct)
    {
        var command = new BusinessRegisterCommand(
            req.Name,
            req.Username,
            req.Email,
            req.Password,
            req.Industry,
            req.BusinessSize,
            req.Address,
            req.ServiceArea,
            req.MakeProfilePublic,
            req.Services,
            req.Website,
            req.PhoneNumber,
            req.Description,
            req.SocialLinks,
            req.Interests,
            req.IsLicensedInNyc,
            req.IsInsured,
            req.OwnershipType
        );
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}