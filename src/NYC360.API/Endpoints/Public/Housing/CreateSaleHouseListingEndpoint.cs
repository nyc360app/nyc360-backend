using NYC360.Application.Features.Housing.Commands.CreateSale;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class CreateSaleHouseListingEndpoint(IMediator mediator) : Endpoint<CreateSaleHousingRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/housing/create/sale");
        Permissions(Domain.Constants.Permissions.Housing.Create);
        AllowFileUploads();
    }
    
    public override async Task HandleAsync(CreateSaleHousingRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var command = new CreateSaleHouseListingCommand(
            userId.Value,
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
            request.Photos,
            request.CoListing,
            request.AllowColisterEditing
        );
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}