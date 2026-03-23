using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NYC360.Domain.Entities.SpaceListings;

namespace NYC360.Infrastructure.Persistence.Configurations.SpaceListings;

public class SpaceListingConfiguration : IEntityTypeConfiguration<SpaceListing>
{
    public void Configure(EntityTypeBuilder<SpaceListing> builder)
    {
        builder.HasKey(x => x.Id);

        ConfigureEnumList(builder, x => x.Categories);
        ConfigureEnumList(builder, x => x.OrganizationServices);
        ConfigureStringList(builder, x => x.Tags);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.ZipCode).HasMaxLength(10);
        builder.Property(x => x.ContactName).HasMaxLength(100);
        builder.Property(x => x.SubmitterNote).HasMaxLength(1000);

        builder.HasMany(x => x.Attachments)
            .WithOne(x => x.SpaceListing)
            .HasForeignKey(x => x.SpaceListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SocialLinks)
            .WithOne(x => x.SpaceListing)
            .HasForeignKey(x => x.SpaceListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Hours)
            .WithOne(x => x.SpaceListing)
            .HasForeignKey(x => x.SpaceListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.ReviewEntries)
            .WithOne(x => x.SpaceListing)
            .HasForeignKey(x => x.SpaceListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureEnumList<TEnum>(EntityTypeBuilder<SpaceListing> builder, System.Linq.Expressions.Expression<Func<SpaceListing, List<TEnum>>> propertyExpression)
        where TEnum : struct, Enum
    {
        var converter = new ValueConverter<List<TEnum>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
            v => JsonSerializer.Deserialize<List<TEnum>>(v, (JsonSerializerOptions)null!) ?? new List<TEnum>()
        );

        var comparer = new ValueComparer<List<TEnum>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );

        builder.Property(propertyExpression)
            .HasConversion(converter)
            .Metadata.SetValueComparer(comparer);

        builder.Property(propertyExpression).HasColumnType("nvarchar(max)");
    }

    private void ConfigureStringList(EntityTypeBuilder<SpaceListing> builder, System.Linq.Expressions.Expression<Func<SpaceListing, List<string>>> propertyExpression)
    {
        var converter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null!) ?? new List<string>()
        );

        var comparer = new ValueComparer<List<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );

        builder.Property(propertyExpression)
            .HasConversion(converter)
            .Metadata.SetValueComparer(comparer);

        builder.Property(propertyExpression).HasColumnType("nvarchar(max)");
    }
}
