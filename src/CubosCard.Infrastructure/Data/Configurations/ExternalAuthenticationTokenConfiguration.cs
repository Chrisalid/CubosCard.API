using CubosCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubosCard.Infrastructure.Data.Configurations;

public class ExternalAuthenticationTokenConfiguration : IEntityTypeConfiguration<ExternalAuthenticationToken>
{
    public void Configure(EntityTypeBuilder<ExternalAuthenticationToken> builder)
    {
        builder.ToTable("ExternalAuthenticationToken");

        builder.HasKey(eat => eat.Id);

        builder.Property(eat => eat.ExternalAuthenticationId)
            .IsRequired();

        builder.Property(eat => eat.ExternalTokenId)
            .IsRequired();

        builder.Property(eat => eat.ExternalAccessToken)
            .IsRequired();

        builder.Property(eat => eat.ExternalRefreshToken)
            .IsRequired();

        builder.HasOne(eat => eat.ExternalAuthentication)
            .WithMany(ea => ea.ExternalAuthenticationTokens)
            .HasForeignKey(eat => eat.ExternalAuthenticationId);

        builder.SetPropertyCommums();
    }
}
