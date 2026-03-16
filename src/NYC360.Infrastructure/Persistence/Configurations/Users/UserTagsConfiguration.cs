using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Tags;

namespace NYC360.Infrastructure.Persistence.Configurations.Users;

public class UserTagsConfiguration : IEntityTypeConfiguration<UserTag>
{
    public void Configure(EntityTypeBuilder<UserTag> builder)
    {
        builder.ToTable("UserTags");

        // Mapping the composite key seen in your logs
        builder.HasKey(ut => new { ut.TagId, ut.UserId });

        // FIX: Change to NoAction or Restrict to prevent circular cascade paths
        builder.HasOne(ut => ut.User)
            .WithMany(u => u.Tags)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.ClientCascade); 
    }
}