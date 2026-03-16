using NYC360.Domain.Entities.Locations;
using NYC360.Domain.Dtos.Location;

namespace NYC360.Application.Contracts.Persistence;

public interface ILocationRepository
{
    Task<Location?> GetOrCreateLocationAsync(LocationDto dto, CancellationToken ct);
    Task<List<Location>> SearchLocationsAsync(string searchTerm, int limit, CancellationToken ct);
    Task<bool> ExistsAsync(int id, CancellationToken ct);
    
    // CRUD Operations for Dashboard
    Task<Location?> GetByIdAsync(int id, CancellationToken ct);
    Task<(List<Location> Items, int TotalCount)> GetPagedLocationsAsync(int page, int pageSize, string? searchTerm, CancellationToken ct);
    Task AddAsync(Location location, CancellationToken ct);
    void Update(Location location);
    void Remove(Location location);
    
    /* Addresses */
    // get or create address
    Task<int> GetOrCreateAddressIdAsync(AddressInputDto dto, CancellationToken ct);
    Task<Address> GetOrCreateAddressAsync(AddressInputDto dto, CancellationToken ct);
    
}