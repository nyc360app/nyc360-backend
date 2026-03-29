using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities;

namespace NYC360.Infrastructure.Persistence.Configurations.Rss;

public class RssFeedItemConfiguration : IEntityTypeConfiguration<RssFeedItem>
{
    public void Configure(EntityTypeBuilder<RssFeedItem> builder)
    {
        builder.ToTable("RssFeedItems");

        builder.Property(x => x.Guid).HasMaxLength(500);
        builder.Property(x => x.Link).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.LinkHash).IsRequired().HasMaxLength(128);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Summary).HasMaxLength(4000);
        builder.Property(x => x.ImageUrl).HasMaxLength(2000);
        builder.Property(x => x.RawMetadataJson).HasMaxLength(4000);

        builder.HasIndex(x => new { x.SourceId, x.LinkHash }).IsUnique();

        builder.HasIndex(x => new { x.SourceId, x.Guid })
            .IsUnique()
            .HasFilter("[Guid] IS NOT NULL");

        builder.HasIndex(x => new { x.Category, x.PublishedAt })
            .IsDescending(false, true);

        builder.HasIndex(x => new { x.SourceId, x.PublishedAt })
            .IsDescending(false, true);

        builder.HasOne<RssFeedSource>()
            .WithMany()
            .HasForeignKey(x => x.SourceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
