using Domain.Entities;
using Domain.ValueObjects;
using Domain.ValueObjects.Name;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
        .HasConversion(n => n.Name, fn => FirstName.Create(fn).Value!)
        .HasMaxLength(100);

        builder.Property(u => u.LastName)
        .HasConversion(n => n.Name, ln => LastName.Create(ln).Value!)
        .HasMaxLength(100);

        builder.Property(u => u.Email)
        .HasConversion(em => em.Mail, em => Email.Create(em).Value!)
        .HasMaxLength(255);

        builder.Property(u => u.DateOfBirth);
        builder.Property(u => u.Phone).HasMaxLength(100);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.Accounts)
        .WithOne()
        .HasForeignKey(a => a.OwnerId);
    }
}
