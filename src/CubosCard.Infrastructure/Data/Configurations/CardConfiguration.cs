using CubosCard.Domain.Constants;
using CubosCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubosCard.Infrastructure.Data.Configurations;

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.AccountId)
            .IsRequired();

        builder.Property(c => c.CardType)
            .IsRequired();

        builder.Property(c => c.CVV)
            .HasMaxLength(FieldLengths.Configuration.CVV)
            .IsRequired();

        builder.Property(c => c.Number)
            .IsRequired();

        builder.HasOne(c => c.Account)
            .WithMany(p => p.Cards)
            .HasForeignKey(a => a.AccountId);

        builder.SetPropertyCommums();
    }
}
