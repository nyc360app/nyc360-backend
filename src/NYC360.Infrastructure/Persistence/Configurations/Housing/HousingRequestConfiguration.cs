using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Housing;

namespace NYC360.Infrastructure.Persistence.Configurations.Housing;

public class HousingRequestConfiguration : IEntityTypeConfiguration<HousingRequest>
{
    public void Configure(EntityTypeBuilder<HousingRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.HouseInfo)
            .WithMany()
            .HasForeignKey(x => x.HouseInfoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
