using CubosCard.Domain.Constants;
using CubosCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubosCard.Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.PersonId)
            .IsRequired();

        builder.Property(a => a.Branch)
            .HasMaxLength(FieldLengths.Configuration.Branch)
            .IsRequired();

        builder.Property(a => a.AccountNumber)
            .IsRequired();

        builder.HasOne(a => a.Person)
            .WithMany(p => p.Accounts)
            .HasForeignKey(a => a.PersonId);

        builder.SetPropertyCommums();
    }
}
