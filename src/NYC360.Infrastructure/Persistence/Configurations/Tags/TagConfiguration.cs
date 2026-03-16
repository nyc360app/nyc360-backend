using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Tags;

namespace NYC360.Infrastructure.Persistence.Configurations.Tags;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

        // Self-referencing relationship (Hierarchy)
        builder.HasOne(t => t.ParentTag)
            .WithMany(t => t.ChildTags)
            .HasForeignKey(t => t.ParentTagId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent accidental cascade deletes of entire branches

        builder.HasMany(t => t.Posts)
            .WithMany(p => p.Tags) // Assuming Post has ICollection<Tag> Tags
            .UsingEntity<PostTag>();
    }
}