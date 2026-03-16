using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Communities;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Persistence.Configurations.Communities;

public class CommunityJoinRequestConfiguration : IEntityTypeConfiguration<CommunityJoinRequest>
{
    public void Configure(EntityTypeBuilder<CommunityJoinRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.CommunityId, x.UserId })
            .IsUnique();

        builder.HasOne(x => x.Community)
            .WithMany()
            .HasForeignKey(x => x.CommunityId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Keeping this is usually fine
    }
}