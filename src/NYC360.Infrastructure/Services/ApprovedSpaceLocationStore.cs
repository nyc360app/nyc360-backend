using Microsoft.EntityFrameworkCore;
using NYC360.Application.Contracts.Infrastructure;
using NYC360.Domain.Entities.SpaceListings;
using NYC360.Infrastructure.Persistence.DbContexts;

namespace NYC360.Infrastructure.Services;

public class ApprovedSpaceLocationStore(ApplicationDbContext context) : IApprovedSpaceLocationStore
{
    public async Task UpsertAsync(SpaceListing listing, int approvedByUserId, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var department = (byte)listing.Department;
        var entityType = (byte)listing.EntityType;

        await context.Database.ExecuteSqlInterpolatedAsync($"""
            IF EXISTS (SELECT 1 FROM [ApprovedSpaceLocationListings] WHERE [SpaceListingId] = {listing.Id})
            BEGIN
                UPDATE [ApprovedSpaceLocationListings]
                SET
                    [Department] = {department},
                    [EntityType] = {entityType},
                    [Name] = {listing.Name},
                    [LocationName] = {listing.LocationName},
                    [Borough] = {listing.Borough},
                    [Neighborhood] = {listing.Neighborhood},
                    [Street] = {listing.Street},
                    [BuildingNumber] = {listing.BuildingNumber},
                    [ZipCode] = {listing.ZipCode},
                    [Website] = {listing.Website},
                    [PhoneNumber] = {listing.PhoneNumber},
                    [PublicEmail] = {listing.PublicEmail},
                    [SubmitterUserId] = {listing.SubmitterUserId},
                    [ApprovedByUserId] = {approvedByUserId},
                    [ApprovedAt] = {now},
                    [ModerationNote] = {listing.ModerationNote},
                    [UpdatedAt] = {now}
                WHERE [SpaceListingId] = {listing.Id};
            END
            ELSE
            BEGIN
                INSERT INTO [ApprovedSpaceLocationListings]
                (
                    [SpaceListingId],
                    [Department],
                    [EntityType],
                    [Name],
                    [LocationName],
                    [Borough],
                    [Neighborhood],
                    [Street],
                    [BuildingNumber],
                    [ZipCode],
                    [Website],
                    [PhoneNumber],
                    [PublicEmail],
                    [SubmitterUserId],
                    [ApprovedByUserId],
                    [ApprovedAt],
                    [ModerationNote],
                    [CreatedAt],
                    [UpdatedAt]
                )
                VALUES
                (
                    {listing.Id},
                    {department},
                    {entityType},
                    {listing.Name},
                    {listing.LocationName},
                    {listing.Borough},
                    {listing.Neighborhood},
                    {listing.Street},
                    {listing.BuildingNumber},
                    {listing.ZipCode},
                    {listing.Website},
                    {listing.PhoneNumber},
                    {listing.PublicEmail},
                    {listing.SubmitterUserId},
                    {approvedByUserId},
                    {now},
                    {listing.ModerationNote},
                    {now},
                    {now}
                );
            END
            """, ct);
    }
}
