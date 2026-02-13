using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity type configuration for the Appointment entity.
/// Defines table mapping, constraints, indexes, and relationships.
/// </summary>
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        // Table mapping
        builder.ToTable("Appointments");

        // Primary key
        builder.HasKey(a => a.AppointmentId);

        // Properties configuration
        builder.Property(a => a.AppointmentId)
            .HasColumnName("AppointmentID")
            .IsRequired();

        builder.Property(a => a.PatientId)
            .HasColumnName("PatientID")
            .IsRequired();

        builder.Property(a => a.DoctorId)
            .HasColumnName("DoctorID")
            .IsRequired();

        builder.Property(a => a.AppointmentDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(a => a.AppointmentTime)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(a => a.Duration)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasMaxLength(20)
            .HasDefaultValue("Scheduled")
            .IsRequired();

        builder.Property(a => a.ReasonForVisit)
            .HasMaxLength(500);

        builder.Property(a => a.Notes)
            .HasColumnType("text");

        builder.Property(a => a.CheckInTime)
            .HasColumnType("datetime");

        builder.Property(a => a.CheckOutTime)
            .HasColumnType("datetime");

        builder.Property(a => a.CreatedDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(a => a.UpdatedDate)
            .HasColumnType("datetime");

        // Indexes
        builder.HasIndex(a => a.PatientId)
            .HasDatabaseName("IX_Appointment_PatientID");

        builder.HasIndex(a => a.DoctorId)
            .HasDatabaseName("IX_Appointment_DoctorID");

        builder.HasIndex(a => a.AppointmentDate)
            .HasDatabaseName("IX_Appointment_Date");

        builder.HasIndex(a => a.Status)
            .HasDatabaseName("IX_Appointment_Status");

        builder.HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.AppointmentTime })
            .HasDatabaseName("IX_Appointment_DoctorDateTime");

        // Relationships
        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Bills)
            .WithOne(b => b.Appointment)
            .HasForeignKey(b => b.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
