using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity type configuration for the Bill entity.
/// Defines table mapping, constraints, indexes, and relationships.
/// </summary>
public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        // Table mapping
        builder.ToTable("Bills");

        // Primary key
        builder.HasKey(b => b.BillId);

        // Properties configuration
        builder.Property(b => b.BillId)
            .HasColumnName("BillID")
            .IsRequired();

        builder.Property(b => b.PatientId)
            .HasColumnName("PatientID")
            .IsRequired();

        builder.Property(b => b.AppointmentId)
            .HasColumnName("AppointmentID");

        builder.Property(b => b.BillDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(b => b.TotalAmount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(b => b.PaidAmount)
            .HasColumnType("decimal(10,2)")
            .HasDefaultValue(0.00m)
            .IsRequired();

        builder.Property(b => b.OutstandingAmount)
            .HasColumnType("decimal(10,2)")
            .HasComputedColumnSql("[TotalAmount] - [PaidAmount]", stored: false);

        builder.Property(b => b.PaymentStatus)
            .HasMaxLength(20)
            .HasDefaultValue("Pending")
            .IsRequired();

        builder.Property(b => b.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(b => b.TransactionId)
            .HasMaxLength(100);

        builder.Property(b => b.Description)
            .HasMaxLength(500);

        builder.Property(b => b.DueDate)
            .HasColumnType("date");

        builder.Property(b => b.PaymentDate)
            .HasColumnType("datetime");

        builder.Property(b => b.CreatedDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(b => b.UpdatedDate)
            .HasColumnType("datetime");

        // Indexes
        builder.HasIndex(b => b.PatientId)
            .HasDatabaseName("IX_Bill_PatientID");

        builder.HasIndex(b => b.AppointmentId)
            .HasDatabaseName("IX_Bill_AppointmentID");

        builder.HasIndex(b => b.BillDate)
            .HasDatabaseName("IX_Bill_Date");

        builder.HasIndex(b => b.PaymentStatus)
            .HasDatabaseName("IX_Bill_PaymentStatus");

        builder.HasIndex(b => b.TransactionId)
            .HasDatabaseName("IX_Bill_TransactionID");

        // Relationships
        builder.HasOne(b => b.Patient)
            .WithMany(p => p.Bills)
            .HasForeignKey(b => b.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Appointment)
            .WithMany(a => a.Bills)
            .HasForeignKey(b => b.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
