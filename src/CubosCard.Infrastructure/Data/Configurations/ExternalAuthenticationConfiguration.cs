using System;
using CubosCard.Domain.Constants;
using CubosCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubosCard.Infrastructure.Data.Configurations;

public class ExternalAuthenticationConfiguration : IEntityTypeConfiguration<ExternalAuthentication>
{
    public void Configure(EntityTypeBuilder<ExternalAuthentication> builder)
    {
        builder.ToTable("ExternalAuthentication");

        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.Email)
            .IsUnique();

        builder.Property(ea => ea.Email)
            .IsRequired();

        builder.Property(a => a.Password)
            .IsRequired();

        builder.Property(a => a.AuthCode)
            .HasMaxLength(FieldLengths.Configuration.AuthCode);

        builder.SetPropertyCommums();
    }
}
