using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Bill entity
/// </summary>
public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("Bills");

        builder.HasKey(b => b.Id);

        builder.HasOne(b => b.Appointment)
            .WithMany()
            .HasForeignKey(b => b.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(b => b.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(b => b.IsPaid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(b => b.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(b => b.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("System");
    }
}
