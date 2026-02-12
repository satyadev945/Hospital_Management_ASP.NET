using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Entity Framework configuration for Feedback entity
    /// </summary>
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            // Table mapping
            builder.ToTable("FeedbackTable");

            // Primary key
            builder.HasKey(f => f.FeedbackID);

            // Properties
            builder.Property(f => f.FeedbackID)
                .HasColumnName("FeedbackID")
                .ValueGeneratedOnAdd();

            builder.Property(f => f.AppointID)
                .HasColumnName("AppointID")
                .IsRequired();

            builder.Property(f => f.PatientID)
                .HasColumnName("PatientID")
                .IsRequired();

            builder.Property(f => f.DoctorID)
                .HasColumnName("DoctorID")
                .IsRequired();

            builder.Property(f => f.FeedbackDate)
                .HasColumnName("FeedbackDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(f => f.Rating)
                .HasColumnName("Rating");

            builder.Property(f => f.Comments)
                .HasColumnName("Comments")
                .HasMaxLength(2000);

            builder.Property(f => f.Status)
                .HasColumnName("Status")
                .HasDefaultValue(2)
                .HasComment("1=Given, 2=Pending");

            builder.Property(f => f.ProfessionalismRating)
                .HasColumnName("ProfessionalismRating");

            builder.Property(f => f.CommunicationRating)
                .HasColumnName("CommunicationRating");

            builder.Property(f => f.TreatmentEffectivenessRating)
                .HasColumnName("TreatmentEffectivenessRating");

            builder.Property(f => f.WaitingTimeRating)
                .HasColumnName("WaitingTimeRating");

            builder.Property(f => f.FacilityRating)
                .HasColumnName("FacilityRating");

            builder.Property(f => f.WouldRecommend)
                .HasColumnName("WouldRecommend");

            builder.Property(f => f.ImprovementSuggestions)
                .HasColumnName("ImprovementSuggestions")
                .HasMaxLength(1000);

            builder.Property(f => f.PositiveAspects)
                .HasColumnName("PositiveAspects")
                .HasMaxLength(1000);

            builder.Property(f => f.IsAnonymous)
                .HasColumnName("IsAnonymous")
                .HasDefaultValue(false);

            builder.Property(f => f.IsReviewed)
                .HasColumnName("IsReviewed")
                .HasDefaultValue(false);

            builder.Property(f => f.ReviewedDate)
                .HasColumnName("ReviewedDate");

            builder.Property(f => f.ReviewedBy)
                .HasColumnName("ReviewedBy")
                .HasMaxLength(100);

            builder.Property(f => f.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            builder.Property(f => f.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(f => f.ModifiedDate)
                .HasColumnName("ModifiedDate");

            builder.Property(f => f.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasMaxLength(100);

            builder.Property(f => f.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .HasMaxLength(100);

            // Ignore computed properties
            builder.Ignore(f => f.IsGiven);
            builder.Ignore(f => f.IsPending);
            builder.Ignore(f => f.IsPositive);
            builder.Ignore(f => f.IsNegative);

            // Relationships
            builder.HasOne(f => f.Appointment)
                .WithOne(a => a.Feedback)
                .HasForeignKey<Feedback>(f => f.AppointID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Patient)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Doctor)
                .WithMany(d => d.Feedbacks)
                .HasForeignKey(f => f.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(f => f.AppointID)
                .IsUnique()
                .HasDatabaseName("IX_Feedback_AppointID");

            builder.HasIndex(f => f.PatientID)
                .HasDatabaseName("IX_Feedback_PatientID");

            builder.HasIndex(f => f.DoctorID)
                .HasDatabaseName("IX_Feedback_DoctorID");

            builder.HasIndex(f => f.Status)
                .HasDatabaseName("IX_Feedback_Status");

            builder.HasIndex(f => f.FeedbackDate)
                .HasDatabaseName("IX_Feedback_FeedbackDate");

            builder.HasIndex(f => f.IsActive)
                .HasDatabaseName("IX_Feedback_IsActive");
        }
    }
}
