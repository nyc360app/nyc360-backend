using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NYC360.Domain.Entities.Forums;

namespace NYC360.Infrastructure.Persistence.Configurations.Forums;

public class ForumConfiguration : IEntityTypeConfiguration<Forum>
{
    public void Configure(EntityTypeBuilder<Forum> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(x => x.Description)
            .HasMaxLength(1000);
            
        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(250);
            
        builder.HasIndex(x => x.Slug)
            .IsUnique();
        
        builder.HasMany(x => x.Questions)
            .WithOne(x => x.Forum)
            .HasForeignKey(x => x.ForumId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
