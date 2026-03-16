using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Enums.Housing;

namespace NYC360.API.Models.Housing;

public record CreateRentHousingRequest(
    HousingType HouseType,
    
    // Availability
    DateTime MoveInDate,
    DateTime? MoveOutDate,
    PropertyType PropertyType,
    
    // Location
    string Borough,
    string ZipCode,
    int MaxOccupants,
    
    //Address
    string Neighborhood,
    string? FullAddress,
    string? UnitNumber,
    string? GoogleMap,
    List<NearbyTransportation>? NearbyTransportation,
    int Bedrooms,
    int Bathrooms,
    int MonthlyRent,
    int? SecurityDeposit,
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
    
    bool ShortTermStayAllowed,
    bool ShortStayEligiblity,
    bool Furnished,
    bool AcceptsHousingVouchers,
    bool FamilyAndKidsFriendly,
    bool PetsFriendly,
    bool AccessibilityFriendly,
    bool SmokingAllowed,
    
    List<RentHousingProgram> AcceptedHousingPrograms,
    string Description,
    
    // Renting Details
    LeaseType LeaseType,
    RentPrivacyType PrivacyType,
    
    // Shared Unit Details
    RentBathroomType SharedBathroomType,
    RentKitchenType SharedKitchenType,
    string? AboutCurrentResident,
    string? UnitRulesAndPolicies,
    string? RoommatesGroupChat,
    bool AddDirectApplyLink,
    
    // Agent Details
    List<IFormFile>? Photos,
    List<int>? CoListing,
    bool? AllowColisterEditing
);