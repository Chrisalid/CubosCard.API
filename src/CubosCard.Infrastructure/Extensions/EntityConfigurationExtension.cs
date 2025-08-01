using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubosCard.Infrastructure.Data.Configurations;

public static class EntityConfigurationExtension
{
    public static EntityTypeBuilder SetPropertyCommums(this EntityTypeBuilder builder)
    {
        builder.Property("CreatedAt")
            .HasColumnName("created_at")
            .HasColumnType("timestamp(6)")
            .IsRequired();

        builder.Property("UpdatedAt")
                .HasColumnName("updated_at")
                .HasColumnType("timestamp(6)")
                .HasDefaultValue(null);

        builder.Property("DeletedAt")
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp(6)");
        
        return builder;
    }
}
