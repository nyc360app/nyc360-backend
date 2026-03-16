using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Housing;

namespace NYC360.Infrastructure.Persistence.Configurations.Housing;

public class HouseListingAuthorizationAvailabilityConfiguration : IEntityTypeConfiguration<HouseListingAuthorizationAvailability>
{
    public void Configure(EntityTypeBuilder<HouseListingAuthorizationAvailability> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.HouseListingAuthorization)
            .WithMany(x => x.Availabilities)
            .HasForeignKey(x => x.HouseListingAuthorizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Dates)
            .HasColumnType("nvarchar(max)");
    }
}
