using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Communities;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Persistence.Configurations.Communities;

public class CommunityMemberConfiguration : IEntityTypeConfiguration<CommunityMember>
{
    public void Configure(EntityTypeBuilder<CommunityMember> builder)
    {
        builder.HasKey(cm => cm.Id);
        
        builder.HasIndex(m => new { m.CommunityId, m.UserId })
            .IsUnique();

        builder.HasOne(m => m.User)
            .WithMany(u => u.CommunityMemberships)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.NoAction); // deleting a user removes memberships

        builder.HasOne(cm => cm.Community)
            .WithMany(c => c.Members)
            .HasForeignKey(cm => cm.CommunityId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete members when community is deleted
    }
}