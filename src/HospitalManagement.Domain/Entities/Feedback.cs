using System;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>
    /// Represents patient feedback for a doctor after an appointment
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// Unique identifier for the feedback
        /// </summary>
        public int FeedbackID { get; set; }

        /// <summary>
        /// Appointment for which feedback is provided (foreign key)
        /// </summary>
        public int AppointID { get; set; }

        /// <summary>
        /// Patient who provided the feedback (foreign key)
        /// </summary>
        public int PatientID { get; set; }

        /// <summary>
        /// Doctor for whom feedback is provided (foreign key)
        /// </summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Date when the feedback was submitted
        /// </summary>
        public DateTime FeedbackDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Rating given to the doctor (typically 1-5 scale)
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// Feedback comments from the patient
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Feedback status (1 = Given, 2 = Pending)
        /// </summary>
        public int Status { get; set; } = 2;

        /// <summary>
        /// Rating for doctor's professionalism
        /// </summary>
        public int? ProfessionalismRating { get; set; }

        /// <summary>
        /// Rating for communication skills
        /// </summary>
        public int? CommunicationRating { get; set; }

        /// <summary>
        /// Rating for treatment effectiveness
        /// </summary>
        public int? TreatmentEffectivenessRating { get; set; }

        /// <summary>
        /// Rating for waiting time satisfaction
        /// </summary>
        public int? WaitingTimeRating { get; set; }

        /// <summary>
        /// Rating for facility cleanliness
        /// </summary>
        public int? FacilityRating { get; set; }

        /// <summary>
        /// Would the patient recommend this doctor to others
        /// </summary>
        public bool? WouldRecommend { get; set; }

        /// <summary>
        /// Areas of improvement suggested by the patient
        /// </summary>
        public string? ImprovementSuggestions { get; set; }

        /// <summary>
        /// Positive aspects highlighted by the patient
        /// </summary>
        public string? PositiveAspects { get; set; }

        /// <summary>
        /// Indicates if the feedback is anonymous
        /// </summary>
        public bool IsAnonymous { get; set; } = false;

        /// <summary>
        /// Indicates if the feedback has been reviewed by management
        /// </summary>
        public bool IsReviewed { get; set; } = false;

        /// <summary>
        /// Date when the feedback was reviewed
        /// </summary>
        public DateTime? ReviewedDate { get; set; }

        /// <summary>
        /// Admin or staff member who reviewed the feedback
        /// </summary>
        public string? ReviewedBy { get; set; }

        /// <summary>
        /// Indicates if the feedback record is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when the feedback was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the feedback was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the feedback record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the feedback record
        /// </summary>
        public string? ModifiedBy { get; set; }

        // Navigation Properties

        /// <summary>
        /// Appointment for which this feedback is provided
        /// </summary>
        public virtual Appointment? Appointment { get; set; }

        /// <summary>
        /// Patient who provided this feedback
        /// </summary>
        public virtual Patient? Patient { get; set; }

        /// <summary>
        /// Doctor who received this feedback
        /// </summary>
        public virtual Doctor? Doctor { get; set; }

        // Helper Methods

        /// <summary>
        /// Checks if feedback has been given
        /// </summary>
        public bool IsGiven => Status == 1;

        /// <summary>
        /// Checks if feedback is pending
        /// </summary>
        public bool IsPending => Status == 2;

        /// <summary>
        /// Calculates the overall average rating
        /// </summary>
        public decimal? GetAverageRating()
        {
            var ratings = new[]
            {
                Rating,
                ProfessionalismRating,
                CommunicationRating,
                TreatmentEffectivenessRating,
                WaitingTimeRating,
                FacilityRating
            };

            var validRatings = ratings.Where(r => r.HasValue).Select(r => r!.Value).ToList();

            if (validRatings.Any())
            {
                return (decimal)validRatings.Average();
            }

            return null;
        }

        /// <summary>
        /// Checks if the feedback is positive (rating >= 4)
        /// </summary>
        public bool IsPositive => Rating.HasValue && Rating.Value >= 4;

        /// <summary>
        /// Checks if the feedback is negative (rating <= 2)
        /// </summary>
        public bool IsNegative => Rating.HasValue && Rating.Value <= 2;
    }
}
