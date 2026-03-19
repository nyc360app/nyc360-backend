using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Infrastructure.Persistence.Configurations.Posts;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasMaxLength(300);

        builder.Property(p => p.Content)
            .HasMaxLength(10_000);

        builder.Property(p => p.Category)
            .IsRequired();

        builder.Property(p => p.PostType)
            .IsRequired();

        builder.Property(p => p.SourceType)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.LastUpdated)
            .IsRequired();

        builder.Property(p => p.ModerationStatus)
            .HasDefaultValue(NYC360.Domain.Enums.Posts.PostModerationStatus.Approved)
            .IsRequired();

        builder.Property(p => p.ModerationNote)
            .HasMaxLength(1000);

        // Post → PostStats (1:1)
        builder.HasOne(p => p.Stats)
            .WithOne(ps => ps.Post)
            .HasForeignKey<PostStats>(ps => ps.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post → Author (User)
        builder.HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        // Post → RssSource (news feed)
        builder.HasOne(p => p.Source)
            .WithMany()
            .HasForeignKey(p => p.SourceId)
            .OnDelete(DeleteBehavior.SetNull);

        // Post → PostLink (1:1)
        builder.HasOne(p => p.Link)
            .WithOne(l => l.Post)
            .HasForeignKey<PostLink>(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Post → Interactions
        builder.HasMany(p => p.Interactions)
            .WithOne(pi => pi.Post)
            .HasForeignKey(pi => pi.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post → Views
        builder.HasMany(p => p.Views)
            .WithOne(v => v.Post)
            .HasForeignKey(v => v.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post → Comments
        builder.HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Post → Community
        builder.HasOne(p => p.Community)
            .WithMany(c => c.Posts)
            .HasForeignKey(p => p.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Post → Tags
        builder.HasMany(p => p.Tags)
            .WithMany(t => t.Posts)
            .UsingEntity(j => j.ToTable("PostTags"));

        // Post → Topic
        builder.HasOne(p => p.Topic)
            .WithMany(t => t.Posts)
            .HasForeignKey(p => p.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.Category, p.ModerationStatus, p.SourceType });
    }
}
