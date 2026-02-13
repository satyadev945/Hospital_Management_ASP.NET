using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity type configuration for the Staff entity.
/// Defines table mapping, constraints, indexes, and relationships.
/// </summary>
public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        // Table mapping
        builder.ToTable("Staff");

        // Primary key
        builder.HasKey(s => s.StaffId);

        // Properties configuration
        builder.Property(s => s.StaffId)
            .HasColumnName("StaffID")
            .IsRequired();

        builder.Property(s => s.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Role)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.DepartmentId)
            .HasColumnName("DepartmentID")
            .IsRequired();

        builder.Property(s => s.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(s => s.HireDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(s => s.Salary)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(s => s.Shift)
            .HasMaxLength(20);

        builder.Property(s => s.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(s => s.Address)
            .HasMaxLength(500);

        builder.Property(s => s.EmergencyContact)
            .HasMaxLength(20);

        builder.Property(s => s.CreatedDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(s => s.UpdatedDate)
            .HasColumnType("datetime");

        // Indexes
        builder.HasIndex(s => s.Email)
            .IsUnique()
            .HasDatabaseName("IX_Staff_Email");

        builder.HasIndex(s => s.DepartmentId)
            .HasDatabaseName("IX_Staff_DepartmentID");

        builder.HasIndex(s => s.Role)
            .HasDatabaseName("IX_Staff_Role");

        builder.HasIndex(s => new { s.LastName, s.FirstName })
            .HasDatabaseName("IX_Staff_Name");

        // Relationships
        builder.HasOne(s => s.Department)
            .WithMany(d => d.Staff)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
