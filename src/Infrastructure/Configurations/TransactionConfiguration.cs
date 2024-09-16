using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Description).HasMaxLength(100);

        builder.HasIndex(t => t.Timestamp);

        builder.HasIndex(t => t.TransactionType);

        builder.Property(t => t.Amount).HasConversion(
            money => money.Amount,
            amount => new Domain.ValueObjects.Money(amount));
    }
}
