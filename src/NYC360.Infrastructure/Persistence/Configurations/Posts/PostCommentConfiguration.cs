using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Infrastructure.Persistence.Configurations.Posts;

public class PostCommentConfiguration  : IEntityTypeConfiguration<PostComment>
{
    public void Configure(EntityTypeBuilder<PostComment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId).IsRequired();
        builder.Property(c => c.Content).IsRequired().HasMaxLength(2000);
        builder.Property(c => c.CreatedAt).IsRequired();
        builder.Property(c => c.UpdatedAt).IsRequired(false);

        // Comment → Post (cascade delete)
        builder.HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Comment → ParentComment (disable cascade delete)
        builder.HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.NoAction);

        // Comment → Stats (1:1)
        builder.HasOne(c => c.Stats)
            .WithOne(s => s.Comment)
            .HasForeignKey<PostCommentStats>(s => s.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Comment → Interactions
        builder.HasMany(c => c.Interactions)
            .WithOne(i => i.Comment)
            .HasForeignKey(i => i.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}