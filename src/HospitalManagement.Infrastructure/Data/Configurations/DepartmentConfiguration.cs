using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Department entity
    /// </summary>
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Table mapping
            builder.ToTable("DepartmentTable");

            // Primary key
            builder.HasKey(d => d.DeptNo);

            // Properties
            builder.Property(d => d.DeptNo)
                .HasColumnName("DeptNo")
                .ValueGeneratedOnAdd();

            builder.Property(d => d.DeptName)
                .HasColumnName("DeptName")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Description)
                .HasColumnName("Description")
                .HasMaxLength(1000);

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

            // Relationships
            builder.HasMany(d => d.Doctors)
                .WithOne(doc => doc.Department)
                .HasForeignKey(doc => doc.DeptNo)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(d => d.DeptName)
                .HasDatabaseName("IX_Department_DeptName");

            builder.HasIndex(d => d.IsActive)
                .HasDatabaseName("IX_Department_IsActive");
        }
    }
}
