using NYC360.Infrastructure.Persistence.DbContexts;
using NYC360.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Dtos.Location;
using NYC360.Domain.Entities;
using NYC360.Domain.Entities.Locations;

namespace NYC360.Infrastructure.Persistence.Repositories;

public sealed class LocationRepository(ApplicationDbContext context) : ILocationRepository
{
    public async Task<Location?> GetOrCreateLocationAsync(LocationDto dto, CancellationToken ct)
    {
        // For now, returning null as per existing implementation or until needed
        return null;
    }
    
    public async Task<List<Location>> SearchLocationsAsync(string searchTerm, int limit, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return new List<Location>();

        var query = GetSearchQuery(searchTerm);

        return await query
            .OrderBy(l => l.Neighborhood)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct)
    {
        return await context.Locations.AnyAsync(l => l.Id == id, ct);
    }

    public async Task<Location?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Locations.FirstOrDefaultAsync(l => l.Id == id, ct);
    }

    public async Task<(List<Location> Items, int TotalCount)> GetPagedLocationsAsync(int page, int pageSize, string? searchTerm, CancellationToken ct)
    {
        var query = context.Locations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = GetSearchQuery(searchTerm);
        }

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderBy(l => l.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task AddAsync(Location location, CancellationToken ct)
    {
        await context.Locations.AddAsync(location, ct);
    }

    public void Update(Location location)
    {
        context.Locations.Update(location);
    }

    public void Remove(Location location)
    {
        context.Locations.Remove(location);
    }

    public async Task<int> GetOrCreateAddressIdAsync(AddressInputDto dto, CancellationToken ct)
    {
        // 1. Check if the provided ID exists and if the data matches
        if (dto.AddressId is > 0)
        {
            var existingById = await context.Addresses.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == dto.AddressId, ct);

            if (existingById != null)
            {
                // If the text data matches exactly, the user is just confirming the old address.
                // We compare trimmed, case-insensitive strings.
                bool isSame = string.Equals(existingById.Street?.Trim(), dto.Street?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                              string.Equals(existingById.BuildingNumber?.Trim(), dto.BuildingNumber?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                              string.Equals(existingById.ZipCode?.Trim(), dto.ZipCode?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                              existingById.LocationId == dto.LocationId;

                if (isSame) return existingById.Id;
            }
        }

        // 2. If we reach here, either the ID was null/invalid OR the user changed the text.
        // We treat this as a lookup for the "New" information to prevent duplicates.
        var street = dto.Street?.Trim();
        var bldNum = dto.BuildingNumber?.Trim();
        var zip = dto.ZipCode?.Trim();

        var existingByText = await context.Addresses
            .FirstOrDefaultAsync(a => 
                a.Street == street && 
                a.BuildingNumber == bldNum && 
                a.ZipCode == zip &&
                a.LocationId == dto.LocationId, 
                ct);

        if (existingByText != null)
        {
            return existingByText.Id;
        }

        // 3. Truly new address - Create it
        var newAddress = new Address
        {
            Street = street,
            BuildingNumber = bldNum,
            ZipCode = zip,
            LocationId = dto.LocationId ?? 0
        };

        context.Addresses.Add(newAddress);
        await context.SaveChangesAsync(ct);

        return newAddress.Id;
    }

    public async Task<Address> GetOrCreateAddressAsync(AddressInputDto dto, CancellationToken ct)
    {
        // 1. Check if the provided ID exists and if the data matches
        if (dto.AddressId is > 0)
        {
            var existingById = await context.Addresses.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == dto.AddressId, ct);

            if (existingById != null)
            {
                // If the text data matches exactly, the user is just confirming the old address.
                // We compare trimmed, case-insensitive strings.
                bool isSame = string.Equals(existingById.Street?.Trim(), dto.Street?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                              string.Equals(existingById.BuildingNumber?.Trim(), dto.BuildingNumber?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                              string.Equals(existingById.ZipCode?.Trim(), dto.ZipCode?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                              existingById.LocationId == dto.LocationId;

                if (isSame) return existingById;
            }
        }

        // 2. If we reach here, either the ID was null/invalid OR the user changed the text.
        // We treat this as a lookup for the "New" information to prevent duplicates.
        var street = dto.Street?.Trim();
        var bldNum = dto.BuildingNumber?.Trim();
        var zip = dto.ZipCode?.Trim();

        var existingByText = await context.Addresses
            .FirstOrDefaultAsync(a => 
                a.Street == street && 
                a.BuildingNumber == bldNum && 
                a.ZipCode == zip &&
                a.LocationId == dto.LocationId, 
                ct);

        if (existingByText != null)
        {
            return existingByText;
        }

        // 3. Truly new address - Create it
        var newAddress = new Address
        {
            Street = street,
            BuildingNumber = bldNum,
            ZipCode = zip,
            LocationId = dto.LocationId ?? 0
        };

        context.Addresses.Add(newAddress);
        await context.SaveChangesAsync(ct);

        return newAddress;
    }

    private IQueryable<Location> GetSearchQuery(string searchTerm)
    {
        var query = context.Locations.AsNoTracking();

        if (int.TryParse(searchTerm, out _))
        {
            query = query.Where(l => l.ZipCode.ToString().Contains(searchTerm));
        }
        else
        {
            query = query.Where(l => 
                (l.Neighborhood != null && l.Neighborhood.Contains(searchTerm)) || 
                (l.NeighborhoodNet != null && l.NeighborhoodNet.Contains(searchTerm)) ||
                (l.Code != null && l.Code.Contains(searchTerm)) ||
                (l.Borough != null && l.Borough.Contains(searchTerm)));
        }

        return query;
    }
}