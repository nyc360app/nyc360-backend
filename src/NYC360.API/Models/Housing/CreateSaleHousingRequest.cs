using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Housing;

namespace NYC360.API.Models.Housing;

public record CreateSaleHousingRequest(
    HousingType HouseType,
    
    // Availability
    DateTime OpeningDate,
    PropertyType PropertyType,
    int LegalUnitCount,
    
    // Location
    string Borough,
    string ZipCode,
    int SuggestedOccupants,
    
    //Address
    string Neighborhood,
    string? FullAddress,
    string? UnitNumber,
    string? GoogleMap,
    List<NearbyTransportation>? NearbyTransportation,
    int Bedrooms,
    int Bathrooms,
    int AskingPrice,
    int? DownPayment,
    int? BrokerFee,
    int? MonthlyCostRange,
    
    // Key details
    BuildingType BuildingType,
    
    int? BuiltIn,
    int? RenovatedIn,
    int? Sqft,
    string? FloorLevel,
    
    HeatingSystem Heating,
    CoolingSystem Cooling,
    TemperatureControl TemperatureControl,
    
    List<LaundryType> Laundry,
    List<HousingAmenities> Amenities,
    
    bool Furnished,
    bool AcceptsHousingVouchers,
    bool FamilyAndKidsFriendly,
    bool PetsFriendly,
    bool AccessibilityFriendly,
    bool SmokingAllowed,
    
    List<BuyerHousingProgram> AcceptedBuyerPrograms,
    string Description,
    bool AddDirectApplyLink,
    
    // Agent Details
    List<IFormFile>? Photos,
    List<int>? CoListing,
    bool? AllowColisterEditing
);