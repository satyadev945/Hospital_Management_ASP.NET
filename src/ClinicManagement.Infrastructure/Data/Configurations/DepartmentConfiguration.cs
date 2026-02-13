using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity type configuration for the Department entity.
/// Defines table mapping, constraints, indexes, and relationships.
/// </summary>
public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Table mapping
        builder.ToTable("Departments");

        // Primary key
        builder.HasKey(d => d.DepartmentId);

        // Properties configuration
        builder.Property(d => d.DepartmentId)
            .HasColumnName("DepartmentID")
            .IsRequired();

        builder.Property(d => d.DepartmentName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.Location)
            .HasMaxLength(100);

        builder.Property(d => d.PhoneExtension)
            .HasMaxLength(10);

        builder.Property(d => d.HeadOfDepartment)
            .HasMaxLength(100);

        builder.Property(d => d.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(d => d.CreatedDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(d => d.UpdatedDate)
            .HasColumnType("datetime");

        // Indexes
        builder.HasIndex(d => d.DepartmentName)
            .IsUnique()
            .HasDatabaseName("IX_Department_Name");

        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_Department_IsActive");

        // Relationships
        builder.HasMany(d => d.Doctors)
            .WithOne(doc => doc.Department)
            .HasForeignKey(doc => doc.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Staff)
            .WithOne(s => s.Department)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
