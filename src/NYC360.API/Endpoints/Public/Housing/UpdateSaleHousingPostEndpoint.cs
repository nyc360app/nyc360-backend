using NYC360.Application.Features.Housing.Commands.UpdateRent;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;
using NYC360.Application.Features.Housing.Commands.UpdateSale;

namespace NYC360.API.Endpoints.Public.Housing;

public class UpdateSaleHousingPostEndpoint(IMediator mediator) : Endpoint<UpdateSaleHousingRequest, StandardResponse>
{
    public override void Configure()
    {
        Put("/housing/{Id}/edit/sale");
        AllowFileUploads();
    }

    public override async Task HandleAsync(UpdateSaleHousingRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        // Map request model to command, including the ID from the route
        var command = new UpdateSaleHouseListingCommand(
            userId.Value,
            request.Id,
            request.HouseType,
            request.OpeningDate,
            request.PropertyType,
            request.LegalUnitCount,
            request.Borough,
            request.ZipCode,
            request.SuggestedOccupants,
            request.Neighborhood,
            request.FullAddress,
            request.UnitNumber,
            request.GoogleMap,
            request.NearbyTransportation,
            request.Bedrooms,
            request.Bathrooms,
            request.AskingPrice,
            request.DownPayment,
            request.BrokerFee,
            request.MonthlyCostRange,
            request.BuildingType,
            request.BuiltIn,
            request.RenovatedIn,
            request.Sqft,
            request.FloorLevel,
            request.Heating,
            request.Cooling,
            request.TemperatureControl,
            request.Laundry,
            request.Amenities,
            request.Furnished,
            request.AcceptsHousingVouchers,
            request.FamilyAndKidsFriendly,
            request.PetsFriendly,
            request.AccessibilityFriendly,
            request.SmokingAllowed,
            request.AcceptedBuyerPrograms,
            request.Description,
            request.AddDirectApplyLink,
            request.NewPhotos,
            request.DeletedPhotoIds,
            request.CoListing,
            request.AllowColisterEditing,
            request.IsPublished
        );
        
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}