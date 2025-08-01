using CubosCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubosCard.Infrastructure.Data.Configurations;

public class AuthTokenConfiguration : IEntityTypeConfiguration<AuthToken>
{
    public void Configure(EntityTypeBuilder<AuthToken> builder)
    {
        builder.HasKey(at => at.Id);

        builder.Property(at => at.PersonId)
            .IsRequired();

        builder.Property(at => at.Token)
            .IsRequired();

        builder.Property(at => at.ExpiresAt)
            .IsRequired();

        builder.HasOne(at => at.Person)
            .WithMany(p => p.AuthTokens)
            .HasForeignKey(a => a.PersonId);

        builder.SetPropertyCommums();
    }
}
