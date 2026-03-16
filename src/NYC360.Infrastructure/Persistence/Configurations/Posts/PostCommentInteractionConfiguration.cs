using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Infrastructure.Persistence.Configurations.Posts;

public class PostCommentInteractionConfiguration : IEntityTypeConfiguration<PostCommentInteraction>
{
    public void Configure(EntityTypeBuilder<PostCommentInteraction> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.UserId).IsRequired();
        builder.Property(i => i.CommentId).IsRequired();
        builder.Property(i => i.Type).IsRequired();
        builder.Property(i => i.CreatedAt).IsRequired();

        // Interaction → User (cascade delete)
        builder.HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Interaction → Comment (disable cascade to avoid multiple cascade paths)
        builder.HasOne(i => i.Comment)
            .WithMany(c => c.Interactions)
            .HasForeignKey(i => i.CommentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}