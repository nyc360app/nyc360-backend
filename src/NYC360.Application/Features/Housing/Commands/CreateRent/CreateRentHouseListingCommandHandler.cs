using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.CreateRent;

public class CreateRentHouseListingCommandHandler(
    IHouseInfoRepository housingRepository,
    ITagRepository tagRepository,
    IUnitOfWork unitOfWork,
    ILocalStorageService storageService)
    : IRequestHandler<CreateRentHouseListingCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateRentHouseListingCommand request, CancellationToken cancellationToken)
    {
        if (request.MoveInDate < DateTime.UtcNow)
        {
            var hasTag = await tagRepository.UserHasTagAsync(request.UserId, "NYC Organization", cancellationToken);
            if (!hasTag) 
                return StandardResponse<int>.Failure(new ApiError("housing.posting.not-eligible", "You are not eligible for early bird pricing."));
        }
        var entity = new HouseInfo
        {
            Description = request.Description,
            // Core Property Info
            IsRenting = true,
            Bedrooms = request.Bedrooms,
            Bathrooms = request.Bathrooms,
            Size = request.Sqft,
            StartingPrice = request.MonthlyRent,
            SecurityDeposit = request.SecurityDeposit,
            BrokerFee = request.BrokerFee,
            MonthlyCostRange = request.MonthlyCostRange,
            
            Borough = request.Borough,
            Neighborhood = request.Neighborhood,
            ZipCode = request.ZipCode,
            FullAddress = request.FullAddress,
    
            // Building & Floor
            BuildingType = request.BuildingType,
            FloorLevel = request.FloorLevel,
            YearBuilt = request.BuiltIn,
            RenovatedIn = request.RenovatedIn,
            MoveOutDate = request.MoveOutDate,
            
            MaxOccupants = request.MaxOccupants,
            UnitNumber = request.UnitNumber,
            GoogleMapLink = request.GoogleMap,
    
            // HVAC & Laundry (From spreadsheet dropdowns)
            HeatingSystem = request.Heating,
            CoolingSystem = request.Cooling,
            TemperatureControl = request.TemperatureControl,
            LaundryTypes = request.Laundry,
    
            // Collections (Stored as JSON-compatible arrays)
            Amenities = request.Amenities,
            NearbyTransportation = request.NearbyTransportation,
            RentHousingPrograms = request.AcceptedHousingPrograms,

            // Amenities & Policies
            IsShortTermStayAllowed = request.ShortTermStayAllowed,
            IsShortStayEligible = request.ShortStayEligiblity,
            IsAcceptsHousingVouchers = request.AcceptsHousingVouchers,
            IsFamilyAndKidsFriendly = request.FamilyAndKidsFriendly,
            IsFurnished = request.Furnished,
            IsPetsFriendly = request.PetsFriendly,
            IsSmokingAllowed = request.SmokingAllowed,
            IsAccessibilityFriendly = request.AccessibilityFriendly,
    
            // Shared Living / Roommate Details
            RentPrivacyType = request.PrivacyType,
            RentKitchenType = request.SharedKitchenType,
            RentBathroomType = request.SharedBathroomType,
            RentingAboutCurrentResident = request.AboutCurrentResident,
            RentingRulesAndPolicies = request.UnitRulesAndPolicies,
            RentingRoommateGroupChat = request.RoommatesGroupChat,
    
            // Logistics
            MoveInDate = request.MoveInDate,
            RentingLeaseType = request.LeaseType,
            AllowColisterEditing = request.AllowColisterEditing,
            AddDirectApplyLink = request.AddDirectApplyLink,
            HouseType = request.HouseType,
            PropertyType = request.PropertyType,
            CoListingIds = request.CoListing,
            IsPublished = false,
            UserId = request.UserId
        };
        if (request.Photos != null && request.Photos.Count != 0)
        {
            foreach (var attachment in request.Photos)
            {
                var fileName = await storageService.SaveFileAsync(attachment, "housing", cancellationToken);
                entity.Attachments.Add(new HousingAttachment { Url = "@local://" + fileName });
            }
        }
        await housingRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return StandardResponse<int>.Success(entity.Id);
    }
}