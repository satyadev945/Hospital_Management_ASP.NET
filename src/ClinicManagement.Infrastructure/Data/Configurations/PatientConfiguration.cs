using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity type configuration for the Patient entity.
/// Defines table mapping, constraints, indexes, and relationships.
/// </summary>
public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        // Table mapping
        builder.ToTable("Patients");

        // Primary key
        builder.HasKey(p => p.PatientId);

        // Properties configuration
        builder.Property(p => p.PatientId)
            .HasColumnName("PatientID")
            .IsRequired();

        builder.Property(p => p.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.DateOfBirth)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(p => p.Gender)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(p => p.Phone)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(p => p.Email)
            .HasMaxLength(255);

        builder.Property(p => p.Address)
            .HasMaxLength(500);

        builder.Property(p => p.EmergencyContact)
            .HasMaxLength(20);

        builder.Property(p => p.BloodGroup)
            .HasMaxLength(5);

        builder.Property(p => p.MedicalHistory)
            .HasColumnType("text");

        builder.Property(p => p.CreatedDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(p => p.UpdatedDate)
            .HasColumnType("datetime");

        // Indexes
        builder.HasIndex(p => p.Phone)
            .HasDatabaseName("IX_Patient_Phone");

        builder.HasIndex(p => p.Email)
            .HasDatabaseName("IX_Patient_Email");

        builder.HasIndex(p => new { p.LastName, p.FirstName })
            .HasDatabaseName("IX_Patient_Name");

        // Relationships
        builder.HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Bills)
            .WithOne(b => b.Patient)
            .HasForeignKey(b => b.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
