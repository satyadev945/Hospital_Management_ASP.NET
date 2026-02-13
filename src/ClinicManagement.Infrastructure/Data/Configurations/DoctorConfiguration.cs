using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity type configuration for the Doctor entity.
/// Defines table mapping, constraints, indexes, and relationships.
/// </summary>
public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        // Table mapping
        builder.ToTable("Doctors");

        // Primary key
        builder.HasKey(d => d.DoctorId);

        // Properties configuration
        builder.Property(d => d.DoctorId)
            .HasColumnName("DoctorID")
            .IsRequired();

        builder.Property(d => d.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Specialization)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(d => d.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.LicenseNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.YearsOfExperience)
            .IsRequired();

        builder.Property(d => d.DepartmentId)
            .HasColumnName("DepartmentID")
            .IsRequired();

        builder.Property(d => d.ConsultationFee)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(d => d.AvailableFrom)
            .HasColumnType("time");

        builder.Property(d => d.AvailableTo)
            .HasColumnType("time");

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
        builder.HasIndex(d => d.LicenseNumber)
            .IsUnique()
            .HasDatabaseName("IX_Doctor_LicenseNumber");

        builder.HasIndex(d => d.Email)
            .HasDatabaseName("IX_Doctor_Email");

        builder.HasIndex(d => d.DepartmentId)
            .HasDatabaseName("IX_Doctor_DepartmentID");

        builder.HasIndex(d => d.Specialization)
            .HasDatabaseName("IX_Doctor_Specialization");

        // Relationships
        builder.HasOne(d => d.Department)
            .WithMany(dept => dept.Doctors)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
