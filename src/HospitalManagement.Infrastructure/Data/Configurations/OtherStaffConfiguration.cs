using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for OtherStaff entity
    /// </summary>
    public class OtherStaffConfiguration : IEntityTypeConfiguration<OtherStaff>
    {
        public void Configure(EntityTypeBuilder<OtherStaff> builder)
        {
            // Table mapping
            builder.ToTable("OtherStaffTable");

            // Primary key
            builder.HasKey(s => s.StaffID);

            // Properties
            builder.Property(s => s.StaffID)
                .HasColumnName("StaffID")
                .ValueGeneratedOnAdd();

            builder.Property(s => s.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Phone)
                .HasColumnName("Phone")
                .HasMaxLength(20);

            builder.Property(s => s.Address)
                .HasColumnName("Address")
                .HasMaxLength(500);

            builder.Property(s => s.Designation)
                .HasColumnName("Designation")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Gender)
                .HasColumnName("Gender")
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(s => s.BirthDate)
                .HasColumnName("BirthDate");

            builder.Property(s => s.HighestQualification)
                .HasColumnName("HighestQualification")
                .HasMaxLength(200);

            builder.Property(s => s.Salary)
                .HasColumnName("Salary")
                .HasPrecision(18, 2);

            builder.Property(s => s.JoiningDate)
                .HasColumnName("JoiningDate");

            builder.Property(s => s.Email)
                .HasColumnName("Email")
                .HasMaxLength(200);

            builder.Property(s => s.EmergencyContact)
                .HasColumnName("EmergencyContact")
                .HasMaxLength(20);

            builder.Property(s => s.EmergencyContactName)
                .HasColumnName("EmergencyContactName")
                .HasMaxLength(200);

            builder.Property(s => s.Department)
                .HasColumnName("Department")
                .HasMaxLength(200);

            builder.Property(s => s.Shift)
                .HasColumnName("Shift")
                .HasMaxLength(50);

            builder.Property(s => s.EmploymentType)
                .HasColumnName("EmploymentType")
                .HasMaxLength(50);

            builder.Property(s => s.EmployeeID)
                .HasColumnName("EmployeeID")
                .HasMaxLength(50);

            builder.Property(s => s.NationalID)
                .HasColumnName("NationalID")
                .HasMaxLength(50);

            builder.Property(s => s.BloodGroup)
                .HasColumnName("BloodGroup")
                .HasMaxLength(10);

            builder.Property(s => s.Status)
                .HasColumnName("Status")
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Active");

            builder.Property(s => s.TerminationDate)
                .HasColumnName("TerminationDate");

            builder.Property(s => s.TerminationReason)
                .HasColumnName("TerminationReason")
                .HasMaxLength(500);

            builder.Property(s => s.Notes)
                .HasColumnName("Notes")
                .HasMaxLength(1000);

            builder.Property(s => s.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            builder.Property(s => s.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(s => s.ModifiedDate)
                .HasColumnName("ModifiedDate");

            builder.Property(s => s.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(100);

            builder.Property(s => s.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .HasMaxLength(100);

            // Ignore computed properties
            builder.Ignore(s => s.Age);
            builder.Ignore(s => s.IsCurrentlyEmployed);
            builder.Ignore(s => s.YearsOfService);
            builder.Ignore(s => s.IsOnProbation);

            // Indexes
            builder.HasIndex(s => s.Email)
                .HasDatabaseName("IX_OtherStaff_Email");

            builder.HasIndex(s => s.EmployeeID)
                .HasDatabaseName("IX_OtherStaff_EmployeeID");

            builder.HasIndex(s => s.Status)
                .HasDatabaseName("IX_OtherStaff_Status");

            builder.HasIndex(s => s.IsActive)
                .HasDatabaseName("IX_OtherStaff_IsActive");
        }
    }
}
