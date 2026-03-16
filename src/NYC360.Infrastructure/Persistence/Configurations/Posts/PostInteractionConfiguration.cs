using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Infrastructure.Persistence.Configurations.Posts;

public class PostInteractionConfiguration : IEntityTypeConfiguration<PostInteraction>
{
    public void Configure(EntityTypeBuilder<PostInteraction> builder)
    {
        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.UserId)
            .IsRequired();

        builder.Property(pi => pi.Type)
            .IsRequired();

        builder.HasOne(pi => pi.Post)
            .WithMany(p => p.Interactions)
            .HasForeignKey(pi => pi.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pi => new { pi.PostId, pi.UserId, pi.Type })
            .IsUnique(); // each user can only have one like/dislike per post
    }
}