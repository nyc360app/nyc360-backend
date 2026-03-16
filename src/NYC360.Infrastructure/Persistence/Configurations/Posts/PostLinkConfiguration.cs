using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Infrastructure.Persistence.Configurations.Posts;

public class PostLinkConfiguration : IEntityTypeConfiguration<PostLink>
{
    public void Configure(EntityTypeBuilder<PostLink> builder)
    {
        builder.HasKey(x => x.PostId);

        builder.Property(x => x.LinkedEntityId)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();

        builder.HasOne(x => x.Post)
            .WithOne(p => p.Link)
            .HasForeignKey<PostLink>(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Index the LinkedEntityId for faster joins in ProfessionsRepository
        builder.HasIndex(pl => new { pl.LinkedEntityId, pl.Type });
    }
}