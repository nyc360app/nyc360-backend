using NYC360.Domain.Entities.SpaceListings;

namespace NYC360.Application.Contracts.Infrastructure;

public interface IApprovedSpaceLocationStore
{
    Task UpsertAsync(SpaceListing listing, int approvedByUserId, CancellationToken ct);
}
