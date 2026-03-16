using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Communities;

namespace NYC360.Infrastructure.Persistence.Configurations.Communities;

public class CommunityDisbandRequestConfiguration : IEntityTypeConfiguration<CommunityDisbandRequest>
{
    public void Configure(EntityTypeBuilder<CommunityDisbandRequest> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Reason).IsRequired().HasMaxLength(1000);
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.RequestedAt).IsRequired();
        builder.Property(r => r.AdminNotes).HasMaxLength(2000);

        // Community relationship: if community is deleted, delete the request
        builder.HasOne(r => r.Community)
            .WithMany()
            .HasForeignKey(r => r.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        // RequestedByUser relationship: restrict delete if user has pending requests
        builder.HasOne(r => r.RequestedByUser)
            .WithMany(up => up.CommunityDisbandRequests)
            .HasForeignKey(r => r.RequestedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ProcessedByUser relationship: restrict delete
        builder.HasOne(r => r.ProcessedByUser)
            .WithMany()
            .HasForeignKey(r => r.ProcessedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for performance
        builder.HasIndex(r => r.CommunityId);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.RequestedAt);
    }
}
