using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Patient entity
    /// </summary>
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            // Table mapping
            builder.ToTable("PatientTable");

            // Primary key
            builder.HasKey(p => p.PatientID);

            // Properties
            builder.Property(p => p.PatientID)
                .HasColumnName("PatientID")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Phone)
                .HasColumnName("Phone")
                .HasMaxLength(20);

            builder.Property(p => p.Address)
                .HasColumnName("Address")
                .HasMaxLength(500);

            builder.Property(p => p.BirthDate)
                .HasColumnName("BirthDate")
                .IsRequired();

            builder.Property(p => p.Gender)
                .HasColumnName("Gender")
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.Email)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Password)
                .HasColumnName("Password")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            builder.Property(p => p.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(p => p.ModifiedDate)
                .HasColumnName("ModifiedDate");

            builder.Property(p => p.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(100);

            builder.Property(p => p.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .HasMaxLength(100);

            // Ignore computed properties
            builder.Ignore(p => p.Age);

            // Relationships
            builder.HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Bills)
                .WithOne(b => b.Patient)
                .HasForeignKey(b => b.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.TreatmentHistories)
                .WithOne(t => t.Patient)
                .HasForeignKey(t => t.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Feedbacks)
                .WithOne(f => f.Patient)
                .HasForeignKey(f => f.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(p => p.Email)
                .IsUnique()
                .HasDatabaseName("IX_Patient_Email");

            builder.HasIndex(p => p.Phone)
                .HasDatabaseName("IX_Patient_Phone");

            builder.HasIndex(p => p.IsActive)
                .HasDatabaseName("IX_Patient_IsActive");
        }
    }
}
