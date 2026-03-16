using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Infrastructure.Persistence.Configurations.Posts;

public class PostCommentStatsConfiguration : IEntityTypeConfiguration<PostCommentStats>
{
    public void Configure(EntityTypeBuilder<PostCommentStats> builder)
    {
        builder.HasKey(cs => cs.CommentId); // 1:1 with comment

        builder.Property(cs => cs.Likes)
            .HasDefaultValue(0);

        builder.Property(cs => cs.Dislikes)
            .HasDefaultValue(0);

        builder.Property(cs => cs.Replies)
            .HasDefaultValue(0);

        builder.HasOne(cs => cs.Comment)
            .WithOne(c => c.Stats)
            .HasForeignKey<PostCommentStats>(cs => cs.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}