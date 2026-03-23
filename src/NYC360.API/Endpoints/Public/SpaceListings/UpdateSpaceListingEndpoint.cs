using FastEndpoints;
using MediatR;
using NYC360.API.Extensions;
using NYC360.API.Models.SpaceListings;
using NYC360.Application.Features.SpaceListings.Commands.Update;
using NYC360.Domain.Wrappers;

namespace NYC360.API.Endpoints.Public.SpaceListings;

public class UpdateSpaceListingEndpoint(IMediator mediator)
    : Endpoint<UpdateSpaceListingRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/space/listings/update/{id}");
        AllowFileUploads();
    }

    public override async Task HandleAsync(UpdateSpaceListingRequest req, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var id = Route<int>("id");
        var command = new UpdateSpaceListingCommand(
            id,
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
            req.Images,
            req.Documents,
            req.OwnershipDocuments,
            req.ProofDocuments
        );

        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
