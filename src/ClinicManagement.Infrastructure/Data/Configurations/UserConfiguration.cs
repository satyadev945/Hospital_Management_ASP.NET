using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for User entity
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.PhoneNo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(u => u.Gender)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(u => u.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.UserType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(u => u.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("System");
    }
}
