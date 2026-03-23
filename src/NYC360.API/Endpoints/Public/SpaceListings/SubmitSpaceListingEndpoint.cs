using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.SpaceListings;
using NYC360.Application.Features.SpaceListings.Commands.Submit;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.SpaceListings;

public class SubmitSpaceListingEndpoint(IMediator mediator)
    : Endpoint<SubmitSpaceListingRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/space/listings/submit");
        Permissions(Domain.Constants.Permissions.SpaceListings.Submit);
        AllowFileUploads();
    }

    public override async Task HandleAsync(SubmitSpaceListingRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new SubmitSpaceListingCommand(
            userId.Value,
            req.Department,
            req.EntityType,
            req.Name,
            req.Description,
            req.Address,
            req.LocationName,
            req.Borough,
            req.Neighborhood,
            req.Website,
            req.PhoneNumber,
            req.PublicEmail,
            req.ContactName,
            req.SubmitterNote,
            req.IsClaimingOwnership,
            req.Categories ?? [],
            req.Tags ?? [],
            req.BusinessIndustry,
            req.BusinessSize,
            req.BusinessServiceArea,
            req.BusinessServices,
            req.BusinessOwnershipType,
            req.BusinessIsLicensedInNyc,
            req.BusinessIsInsured,
            req.OrganizationType,
            req.OrganizationFundType,
            req.OrganizationServices ?? [],
            req.OrganizationIsTaxExempt,
            req.OrganizationIsNysRegistered,
            req.SocialLinks?.Select(x => new Application.Features.SpaceListings.Commands.Submit.SpaceListingSocialLinkInput(x.Platform, x.Url)).ToList() ?? [],
            req.Hours?.Select(x => new Application.Features.SpaceListings.Commands.Submit.SpaceListingHourInput(x.DayOfWeek, x.OpenTime, x.CloseTime, x.IsClosed)).ToList() ?? [],
            req.SaveAsDraft,
            req.Images,
            req.Documents,
            req.OwnershipDocuments,
            req.ProofDocuments
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
