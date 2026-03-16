using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NYC360.Domain.Entities.Housing;

namespace NYC360.Infrastructure.Persistence.Configurations.Housing;

public class HouseInfoConfiguration : IEntityTypeConfiguration<HouseInfo>
{
    public void Configure(EntityTypeBuilder<HouseInfo> builder)
    {
        builder.HasKey(x => x.Id);

        ConfigureEnumList(builder, x => x.LaundryTypes);
        ConfigureEnumList(builder, x => x.RentHousingPrograms);
        ConfigureEnumList(builder, x => x.BuyerHousingProgram);
        ConfigureEnumList(builder, x => x.NearbyTransportation);
        ConfigureEnumList(builder, x => x.Amenities);

        // Standard primitive list
        builder.Property(x => x.CoListingIds)
            .HasColumnType("nvarchar(max)");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Attachments)
            .WithOne(x => x.Housing)
            .HasForeignKey(x => x.HousingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Borough).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ZipCode).IsRequired().HasMaxLength(10);
    }
    
    private void ConfigureEnumList<TEnum>(EntityTypeBuilder<HouseInfo> builder, System.Linq.Expressions.Expression<Func<HouseInfo, List<TEnum>?>> propertyExpression) 
        where TEnum : struct, Enum
    {
        // 1. Value Converter
        var converter = new ValueConverter<List<TEnum>?, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
            v => JsonSerializer.Deserialize<List<TEnum>>(v, (JsonSerializerOptions)null!) ?? new List<TEnum>()
        );

        // 2. Value Comparer (Tells EF how to check if the list changed)
        var comparer = new ValueComparer<List<TEnum>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );

        builder.Property(propertyExpression)
            .HasConversion(converter)
            .Metadata.SetValueComparer(comparer);

        builder.Property(propertyExpression).HasColumnType("nvarchar(max)"); 
    }
}