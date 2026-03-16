using NYC360.Application.Features.Housing.Commands.CreateRent;
using NYC360.API.Models.Housing;
using NYC360.Domain.Wrappers;
using NYC360.API.Extensions;
using FastEndpoints;
using MediatR;

namespace NYC360.API.Endpoints.Public.Housing;

public class CreateRentHouseListingEndpoint(IMediator mediator) : Endpoint<CreateRentHousingRequest, StandardResponse<int>>
{
    public override void Configure()
    {
        Post("/housing/create/renting");
        Permissions(Domain.Constants.Permissions.Housing.Create);
        AllowFileUploads();
    }
    
    public override async Task HandleAsync(CreateRentHousingRequest request, CancellationToken ct)
    {
        var userId = User.GetId();
        if (userId == null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var command = new CreateRentHouseListingCommand(
            userId.Value,
            request.HouseType,
            request.MoveInDate,
            request.MoveOutDate,
            request.PropertyType,
            request.Borough,
            request.ZipCode,
            request.MaxOccupants,
            request.Neighborhood,
            request.FullAddress,
            request.UnitNumber,
            request.GoogleMap,
            request.NearbyTransportation,
            request.Bedrooms,
            request.Bathrooms,
            request.MonthlyRent,
            request.SecurityDeposit,
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
            request.ShortTermStayAllowed,
            request.ShortStayEligiblity,
            request.Furnished,
            request.AcceptsHousingVouchers,
            request.FamilyAndKidsFriendly,
            request.PetsFriendly,
            request.AccessibilityFriendly,
            request.SmokingAllowed,
            request.AcceptedHousingPrograms,
            request.Description,
            request.LeaseType,
            request.PrivacyType,
            request.SharedBathroomType,
            request.SharedKitchenType,
            request.AboutCurrentResident,
            request.UnitRulesAndPolicies,
            request.RoommatesGroupChat,
            request.AddDirectApplyLink,
            request.Photos,
            request.CoListing,
            request.AllowColisterEditing
        );
        var result = await mediator.Send(command, ct);
        await Send.OkAsync(result, cancellation: ct);
    }
}