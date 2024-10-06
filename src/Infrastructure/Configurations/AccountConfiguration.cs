using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class AccountConfiguration() : IEntityTypeConfiguration<Account>
{

    public void Configure(EntityTypeBuilder<Account> builder)
    {

        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.AccountNumber).IsUnique();

        builder.Property(a => a.AccountNumber).HasMaxLength(100);

        builder.Property(a => a.Balance).HasConversion(
            money => money.Value,
            amount => Domain.ValueObjects.Money.Create(amount).Value!);

        builder.HasOne<User>()
        .WithMany()
        .HasForeignKey(a => a.OwnerId)
        .IsRequired();

        builder.HasMany(a => a.Transactions)
        .WithOne()
        .HasForeignKey(t => t.AccountId);
    }
}
