using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Doctor entity
    /// </summary>
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            // Table mapping
            builder.ToTable("DoctorTable");

            // Primary key
            builder.HasKey(d => d.DoctorID);

            // Properties
            builder.Property(d => d.DoctorID)
                .HasColumnName("DoctorID")
                .ValueGeneratedOnAdd();

            builder.Property(d => d.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Phone)
                .HasColumnName("Phone")
                .HasMaxLength(20);

            builder.Property(d => d.Address)
                .HasColumnName("Address")
                .HasMaxLength(500);

            builder.Property(d => d.BirthDate)
                .HasColumnName("BirthDate")
                .IsRequired();

            builder.Property(d => d.Gender)
                .HasColumnName("Gender")
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(d => d.DeptNo)
                .HasColumnName("DeptNo")
                .IsRequired();

            builder.Property(d => d.ChargesPerVisit)
                .HasColumnName("ChargesPerVisit")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(d => d.MonthlySalary)
                .HasColumnName("MonthlySalary")
                .HasPrecision(18, 2);

            builder.Property(d => d.ReputeIndex)
                .HasColumnName("ReputeIndex")
                .HasPrecision(5, 2);

            builder.Property(d => d.PatientsTreated)
                .HasColumnName("PatientsTreated")
                .HasDefaultValue(0);

            builder.Property(d => d.Qualification)
                .HasColumnName("Qualification")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(d => d.Specialization)
                .HasColumnName("Specialization")
                .HasMaxLength(200);

            builder.Property(d => d.WorkExperience)
                .HasColumnName("WorkExperience");

            builder.Property(d => d.Status)
                .HasColumnName("Status")
                .HasDefaultValue(1);

            builder.Property(d => d.Email)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Password)
                .HasColumnName("Password")
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(d => d.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            builder.Property(d => d.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(d => d.ModifiedDate)
                .HasColumnName("ModifiedDate");

            builder.Property(d => d.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(100);

            builder.Property(d => d.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .HasMaxLength(100);

            // Ignore computed properties
            builder.Ignore(d => d.Age);

            // Relationships
            builder.HasOne(d => d.Department)
                .WithMany(dept => dept.Doctors)
                .HasForeignKey(d => d.DeptNo)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.TreatmentHistories)
                .WithOne(t => t.Doctor)
                .HasForeignKey(t => t.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Feedbacks)
                .WithOne(f => f.Doctor)
                .HasForeignKey(f => f.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(d => d.Email)
                .IsUnique()
                .HasDatabaseName("IX_Doctor_Email");

            builder.HasIndex(d => d.DeptNo)
                .HasDatabaseName("IX_Doctor_DeptNo");

            builder.HasIndex(d => d.Status)
                .HasDatabaseName("IX_Doctor_Status");

            builder.HasIndex(d => d.IsActive)
                .HasDatabaseName("IX_Doctor_IsActive");
        }
    }
}
