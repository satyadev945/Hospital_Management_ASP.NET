using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for TreatmentHistory entity
    /// </summary>
    public class TreatmentHistoryConfiguration : IEntityTypeConfiguration<TreatmentHistory>
    {
        public void Configure(EntityTypeBuilder<TreatmentHistory> builder)
        {
            // Table mapping
            builder.ToTable("TreatmentHistoryTable");

            // Primary key
            builder.HasKey(t => t.TreatmentHistoryID);

            // Properties
            builder.Property(t => t.TreatmentHistoryID)
                .HasColumnName("TreatmentHistoryID")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.AppointID)
                .HasColumnName("AppointID")
                .IsRequired();

            builder.Property(t => t.PatientID)
                .HasColumnName("PatientID")
                .IsRequired();

            builder.Property(t => t.DoctorID)
                .HasColumnName("DoctorID")
                .IsRequired();

            builder.Property(t => t.TreatmentDate)
                .HasColumnName("TreatmentDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(t => t.Disease)
                .HasColumnName("Disease")
                .HasMaxLength(500);

            builder.Property(t => t.Symptoms)
                .HasColumnName("Symptoms")
                .HasMaxLength(1000);

            builder.Property(t => t.Diagnosis)
                .HasColumnName("Diagnosis")
                .HasMaxLength(2000);

            builder.Property(t => t.TreatmentPlan)
                .HasColumnName("TreatmentPlan")
                .HasMaxLength(2000);

            builder.Property(t => t.Prescription)
                .HasColumnName("Prescription")
                .HasMaxLength(2000);

            builder.Property(t => t.Progress)
                .HasColumnName("Progress")
                .HasMaxLength(2000);

            builder.Property(t => t.FollowUpInstructions)
                .HasColumnName("FollowUpInstructions")
                .HasMaxLength(1000);

            builder.Property(t => t.NextFollowUpDate)
                .HasColumnName("NextFollowUpDate");

            builder.Property(t => t.LabResults)
                .HasColumnName("LabResults")
                .HasMaxLength(2000);

            builder.Property(t => t.VitalSigns)
                .HasColumnName("VitalSigns")
                .HasMaxLength(1000);

            builder.Property(t => t.Notes)
                .HasColumnName("Notes")
                .HasMaxLength(2000);

            builder.Property(t => t.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            builder.Property(t => t.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(t => t.ModifiedDate)
                .HasColumnName("ModifiedDate");

            builder.Property(t => t.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(100);

            builder.Property(t => t.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .HasMaxLength(100);

            // Ignore computed properties
            builder.Ignore(t => t.IsFollowUpDue);
            builder.Ignore(t => t.DaysSinceTreatment);

            // Relationships
            builder.HasOne(t => t.Appointment)
                .WithOne(a => a.TreatmentHistory)
                .HasForeignKey<TreatmentHistory>(t => t.AppointID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Patient)
                .WithMany(p => p.TreatmentHistories)
                .HasForeignKey(t => t.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Doctor)
                .WithMany(d => d.TreatmentHistories)
                .HasForeignKey(t => t.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(t => t.AppointID)
                .IsUnique()
                .HasDatabaseName("IX_TreatmentHistory_AppointID");

            builder.HasIndex(t => t.PatientID)
                .HasDatabaseName("IX_TreatmentHistory_PatientID");

            builder.HasIndex(t => t.DoctorID)
                .HasDatabaseName("IX_TreatmentHistory_DoctorID");

            builder.HasIndex(t => t.TreatmentDate)
                .HasDatabaseName("IX_TreatmentHistory_TreatmentDate");

            builder.HasIndex(t => t.IsActive)
                .HasDatabaseName("IX_TreatmentHistory_IsActive");
        }
    }
}
