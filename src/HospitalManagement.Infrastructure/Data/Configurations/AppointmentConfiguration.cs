using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Appointment entity
    /// </summary>
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            // Table mapping
            builder.ToTable("AppointmentTable");

            // Primary key
            builder.HasKey(a => a.AppointID);

            // Properties
            builder.Property(a => a.AppointID)
                .HasColumnName("AppointID")
                .ValueGeneratedOnAdd();

            builder.Property(a => a.DoctorID)
                .HasColumnName("DoctorID");

            builder.Property(a => a.PatientID)
                .HasColumnName("PatientID");

            builder.Property(a => a.Date)
                .HasColumnName("Date");

            builder.Property(a => a.AppointmentStatus)
                .HasColumnName("AppointmentStatus")
                .HasComment("1=Approved, 2=Pending, 3=Completed, 4=Rejected");

            builder.Property(a => a.BillAmount)
                .HasColumnName("BillAmount")
                .HasPrecision(18, 2);

            builder.Property(a => a.BillStatus)
                .HasColumnName("BillStatus")
                .HasMaxLength(50);

            builder.Property(a => a.DoctorNotification)
                .HasColumnName("DoctorNotification")
                .HasComment("1=Seen, 2=Unseen");

            builder.Property(a => a.PatientNotification)
                .HasColumnName("PatientNotification")
                .HasComment("1=Seen, 2=Unseen");

            builder.Property(a => a.FeedbackStatus)
                .HasColumnName("FeedbackStatus")
                .HasComment("1=Given, 2=Pending");

            builder.Property(a => a.Disease)
                .HasColumnName("Disease")
                .HasMaxLength(500);

            builder.Property(a => a.Progress)
                .HasColumnName("Progress")
                .HasMaxLength(2000);

            builder.Property(a => a.Prescription)
                .HasColumnName("Prescription")
                .HasMaxLength(2000);

            builder.Property(a => a.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.ModifiedDate)
                .HasColumnName("ModifiedDate");

            builder.Property(a => a.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(100);

            builder.Property(a => a.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .HasMaxLength(100);

            // Ignore computed properties
            builder.Ignore(a => a.IsPending);
            builder.Ignore(a => a.IsApproved);
            builder.Ignore(a => a.IsCompleted);
            builder.Ignore(a => a.IsRejected);
            builder.Ignore(a => a.IsBillPaid);

            // Relationships
            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Bill)
                .WithOne(b => b.Appointment)
                .HasForeignKey<Bill>(b => b.AppointID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.TreatmentHistory)
                .WithOne(t => t.Appointment)
                .HasForeignKey<TreatmentHistory>(t => t.AppointID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Feedback)
                .WithOne(f => f.Appointment)
                .HasForeignKey<Feedback>(f => f.AppointID)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(a => a.DoctorID)
                .HasDatabaseName("IX_Appointment_DoctorID");

            builder.HasIndex(a => a.PatientID)
                .HasDatabaseName("IX_Appointment_PatientID");

            builder.HasIndex(a => a.Date)
                .HasDatabaseName("IX_Appointment_Date");

            builder.HasIndex(a => a.AppointmentStatus)
                .HasDatabaseName("IX_Appointment_Status");

            builder.HasIndex(a => a.IsActive)
                .HasDatabaseName("IX_Appointment_IsActive");
        }
    }
}
