using CubosCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubosCard.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.AccountId)
            .IsRequired();

        builder.Property(c => c.Value)
            .IsRequired();

        builder.Property(c => c.Description)
            .IsRequired();

        builder.HasOne(c => c.Account)
            .WithMany(p => p.Transactions)
            .HasForeignKey(a => a.AccountId);

        builder.SetPropertyCommums();
    }
}
