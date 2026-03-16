using NYC360.Application.Contracts.Persistence;
using NYC360.Application.Contracts.Storage;
using NYC360.Domain.Entities.Housing;
using NYC360.Domain.Wrappers;
using MediatR;

namespace NYC360.Application.Features.Housing.Commands.CreateSale;

public class CreateSaleHouseListingCommandHandler(
    IHouseInfoRepository housingRepository,
    ITagRepository tagRepository,
    IUnitOfWork unitOfWork,
    ILocalStorageService storageService)
    : IRequestHandler<CreateSaleHouseListingCommand, StandardResponse<int>>
{
    public async Task<StandardResponse<int>> Handle(CreateSaleHouseListingCommand request, CancellationToken cancellationToken)
    {
        if (request.OpeningDate < DateTime.UtcNow)
        {
            var hasTag = await tagRepository.UserHasTagAsync(request.UserId, "NYC Organization", cancellationToken);
            if (!hasTag) 
                return StandardResponse<int>.Failure(new ApiError("housing.posting.not-eligible", "You are not eligible for early bird pricing."));
        }
        var entity = new HouseInfo
        {
            Description = request.Description,
            // Core Property Info
            IsRenting = false,
            Bedrooms = request.Bedrooms,
            Bathrooms = request.Bathrooms,
            Size = request.Sqft,
            StartingPrice = request.AskingPrice,
            SecurityDeposit = request.DownPayment,
            BrokerFee = request.BrokerFee,
            MonthlyCostRange = request.MonthlyCostRange,
            LegalUnitCount = request.LegalUnitCount,
            
            Borough = request.Borough,
            Neighborhood = request.Neighborhood,
            ZipCode = request.ZipCode,
            FullAddress = request.FullAddress,
    
            // Building & Floor
            BuildingType = request.BuildingType,
            FloorLevel = request.FloorLevel,
            YearBuilt = request.BuiltIn,
            RenovatedIn = request.RenovatedIn,
            
            MaxOccupants = request.SuggestedOccupants,
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
            BuyerHousingProgram = request.AcceptedBuyerPrograms,

            // Amenities & Policies
            IsAcceptsHousingVouchers = request.AcceptsHousingVouchers,
            IsFamilyAndKidsFriendly = request.FamilyAndKidsFriendly,
            IsFurnished = request.Furnished,
            IsPetsFriendly = request.PetsFriendly,
            IsSmokingAllowed = request.SmokingAllowed,
            IsAccessibilityFriendly = request.AccessibilityFriendly,
    
            // Logistics
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