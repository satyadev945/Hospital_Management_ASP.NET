using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Bill entity
    /// </summary>
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            // Table mapping
            builder.ToTable("BillTable");

            // Primary key
            builder.HasKey(b => b.BillID);

            // Properties
            builder.Property(b => b.BillID)
                .HasColumnName("BillID")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.AppointID)
                .HasColumnName("AppointID")
                .IsRequired();

            builder.Property(b => b.PatientID)
                .HasColumnName("PatientID")
                .IsRequired();

            builder.Property(b => b.DoctorID)
                .HasColumnName("DoctorID");

            builder.Property(b => b.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(b => b.Status)
                .HasColumnName("Status")
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Unpaid");

            builder.Property(b => b.BillDate)
                .HasColumnName("BillDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(b => b.PaymentDate)
                .HasColumnName("PaymentDate");

            builder.Property(b => b.PaymentMethod)
                .HasColumnName("PaymentMethod")
                .HasMaxLength(50);

            builder.Property(b => b.TransactionReference)
                .HasColumnName("TransactionReference")
                .HasMaxLength(200);

            builder.Property(b => b.Notes)
                .HasColumnName("Notes")
                .HasMaxLength(1000);

            builder.Property(b => b.ConsultationCharges)
                .HasColumnName("ConsultationCharges")
                .HasPrecision(18, 2);

            builder.Property(b => b.MedicationCharges)
                .HasColumnName("MedicationCharges")
                .HasPrecision(18, 2);

            builder.Property(b => b.LabCharges)
                .HasColumnName("LabCharges")
                .HasPrecision(18, 2);

            builder.Property(b => b.OtherCharges)
                .HasColumnName("OtherCharges")
                .HasPrecision(18, 2);

            builder.Property(b => b.Discount)
                .HasColumnName("Discount")
                .HasPrecision(18, 2);

            builder.Property(b => b.Tax)
                .HasColumnName("Tax")
                .HasPrecision(18, 2);

            builder.Property(b => b.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            builder.Property(b => b.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(b => b.ModifiedDate)
                .HasColumnName("ModifiedDate");

            builder.Property(b => b.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(100);

            builder.Property(b => b.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .HasMaxLength(100);

            // Ignore computed properties
            builder.Ignore(b => b.IsPaid);
            builder.Ignore(b => b.IsOverdue);

            // Relationships
            builder.HasOne(b => b.Appointment)
                .WithOne(a => a.Bill)
                .HasForeignKey<Bill>(b => b.AppointID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Patient)
                .WithMany(p => p.Bills)
                .HasForeignKey(b => b.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Doctor)
                .WithMany()
                .HasForeignKey(b => b.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(b => b.AppointID)
                .IsUnique()
                .HasDatabaseName("IX_Bill_AppointID");

            builder.HasIndex(b => b.PatientID)
                .HasDatabaseName("IX_Bill_PatientID");

            builder.HasIndex(b => b.Status)
                .HasDatabaseName("IX_Bill_Status");

            builder.HasIndex(b => b.BillDate)
                .HasDatabaseName("IX_Bill_BillDate");

            builder.HasIndex(b => b.IsActive)
                .HasDatabaseName("IX_Bill_IsActive");
        }
    }
}
