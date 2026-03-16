namespace NYC360.Domain.Dtos.Location;

public record AddressInputDto(
    int? AddressId,
    int? LocationId,
    string? Street,
    string? BuildingNumber,
    string? ZipCode
);