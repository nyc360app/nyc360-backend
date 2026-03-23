using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.SpaceListings;

namespace NYC360.Infrastructure.Persistence.Configurations.SpaceListings;

public class SpaceListingReviewEntryConfiguration : IEntityTypeConfiguration<SpaceListingReviewEntry>
{
    public void Configure(EntityTypeBuilder<SpaceListingReviewEntry> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.SpaceListing)
            .WithMany(x => x.ReviewEntries)
            .HasForeignKey(x => x.SpaceListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Reviewer)
            .WithMany()
            .HasForeignKey(x => x.ReviewerUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
