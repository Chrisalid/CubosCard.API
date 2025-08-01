using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubosCard.Infrastructure.Data.Configurations;

public static class EntityConfigurationExtension
{
    public static EntityTypeBuilder SetPropertyCommums(this EntityTypeBuilder builder)
    {
        builder.Property("CreatedAt")
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp(6)")
            .IsRequired();

        builder.Property("UpdatedAt")
                .HasColumnName("UpdatedAt")
                .HasColumnType("timestamp(6)")
                .HasDefaultValue(null);

        builder.Property("DeletedAt")
                .HasColumnName("DeletedAt")
                .HasColumnType("timestamp(6)")
                .IsRequired();
        
        return builder;
    }
}
