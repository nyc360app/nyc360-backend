using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Communities;
using Microsoft.EntityFrameworkCore;

namespace NYC360.Infrastructure.Persistence.Configurations.Communities;

public class CommunityConfiguration : IEntityTypeConfiguration<Community>
{
    public void Configure(EntityTypeBuilder<Community> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Slug).IsUnique();

        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Slug).IsRequired();
        builder.Property(c => c.Description).IsRequired();
        builder.Property(c => c.IsActive).HasDefaultValue(true);

        // Location: no cascade delete
        builder.HasOne(c => c.Location)
            .WithMany()
            .HasForeignKey(c => c.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Members: break cascade
        builder.HasMany(c => c.Members)
            .WithOne(m => m.Community)
            .HasForeignKey(m => m.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Posts: break cascade
        builder.HasMany(c => c.Posts)
            .WithOne(p => p.Community)
            .HasForeignKey(p => p.CommunityId)
            .OnDelete(DeleteBehavior.Cascade); // important
    }
}