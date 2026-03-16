using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Housing;

namespace NYC360.Infrastructure.Persistence.Configurations.Housing;

public class HouseListingAuthorizationConfiguration : IEntityTypeConfiguration<HouseListingAuthorization>
{
    public void Configure(EntityTypeBuilder<HouseListingAuthorization> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Avoid cycle with HouseInfo -> User path

        builder.HasOne<HouseInfo>()
            .WithOne(x => x.HouseListingAuthorization)
            .HasForeignKey<HouseListingAuthorization>(x => x.HouseInfoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
