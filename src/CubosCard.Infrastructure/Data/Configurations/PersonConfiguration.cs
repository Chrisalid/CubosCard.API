using CubosCard.Domain.Constants;
using CubosCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CubosCard.Infrastructure.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired();

        builder.Property(p => p.Document)
            .HasMaxLength(FieldLengths.Configuration.Document)
            .IsRequired();

        builder.Property(p => p.Password)
            .IsRequired();

        builder.SetPropertyCommums();
    }
}
