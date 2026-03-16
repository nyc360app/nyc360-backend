namespace NYC360.Domain.Enums.Housing;

public enum NearbyTransportation
{
    // Subway Lines
    Subway_123 = 0,
    Subway_456 = 1,
    Subway_7 = 2,
    Subway_ACE = 3,
    Subway_BDFM = 4,
    Subway_G = 5,
    Subway_JZ = 6,
    Subway_L = 7,
    Subway_NQRW = 8,
    Subway_S = 9,

    // Bus Access
    Bus_Local = 10,
    Bus_Limited = 11,
    Bus_SBS = 12,
    Bus_24Hour = 13,

    // Regional Rail
    Rail_LIRR = 14,
    Rail_MetroNorth = 15,
    Rail_PATH = 16,

    // Ferry
    Ferry_NYC = 17,
    Ferry_StatenIsland = 18,

    // Bike & Micromobility
    Bike_CitiBike = 19,
    Bike_ProtectedLanes = 20,
    Bike_ScooterFriendly = 21,

    // Car Access
    Car_HighwayAccess = 22,
    Car_StreetParking = 23,
    Car_GarageParking = 24
}