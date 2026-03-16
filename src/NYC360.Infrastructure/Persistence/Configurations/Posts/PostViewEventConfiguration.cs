using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NYC360.Domain.Entities.Posts;

namespace NYC360.Infrastructure.Persistence.Configurations.Posts;

public class PostViewEventConfiguration : IEntityTypeConfiguration<PostViewEvent>
{
    public void Configure(EntityTypeBuilder<PostViewEvent> builder)
    {
        builder.HasKey(pv => pv.Id);

        builder.Property(pv => pv.UserId)
            .IsRequired();

        // builder.Property(pv => pv.Date)
        //     .IsRequired();

        builder.HasOne(pv => pv.Post)
            .WithMany(p => p.Views)
            .HasForeignKey(pv => pv.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pv => new { pv.PostId, pv.UserId })
            .IsUnique(); // each user can only have one view record per post
    }
}