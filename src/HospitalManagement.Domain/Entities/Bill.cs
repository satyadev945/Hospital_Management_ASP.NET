using System;

namespace HospitalManagement.Domain.Entities
{
    /// <summary>
    /// Represents a bill for medical services rendered
    /// </summary>
    public class Bill
    {
        /// <summary>
        /// Unique identifier for the bill
        /// </summary>
        public int BillID { get; set; }

        /// <summary>
        /// Appointment associated with this bill (foreign key)
        /// </summary>
        public int AppointID { get; set; }

        /// <summary>
        /// Patient who is responsible for this bill (foreign key)
        /// </summary>
        public int PatientID { get; set; }

        /// <summary>
        /// Doctor who provided the service (foreign key)
        /// </summary>
        public int? DoctorID { get; set; }

        /// <summary>
        /// Total amount to be paid
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Payment status (Paid/Unpaid)
        /// </summary>
        public string Status { get; set; } = "Unpaid";

        /// <summary>
        /// Date when the bill was generated
        /// </summary>
        public DateTime BillDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when the bill was paid (if applicable)
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Payment method used (Cash, Card, Insurance, etc.)
        /// </summary>
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// Transaction reference number
        /// </summary>
        public string? TransactionReference { get; set; }

        /// <summary>
        /// Additional notes or remarks about the bill
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Consultation charges
        /// </summary>
        public decimal? ConsultationCharges { get; set; }

        /// <summary>
        /// Medication charges
        /// </summary>
        public decimal? MedicationCharges { get; set; }

        /// <summary>
        /// Laboratory test charges
        /// </summary>
        public decimal? LabCharges { get; set; }

        /// <summary>
        /// Other miscellaneous charges
        /// </summary>
        public decimal? OtherCharges { get; set; }

        /// <summary>
        /// Discount applied to the bill
        /// </summary>
        public decimal? Discount { get; set; }

        /// <summary>
        /// Tax amount applied
        /// </summary>
        public decimal? Tax { get; set; }

        /// <summary>
        /// Indicates if the bill record is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date and time when the bill was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the bill was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the bill record
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the bill record
        /// </summary>
        public string? ModifiedBy { get; set; }

        // Navigation Properties

        /// <summary>
        /// Appointment associated with this bill
        /// </summary>
        public virtual Appointment? Appointment { get; set; }

        /// <summary>
        /// Patient associated with this bill
        /// </summary>
        public virtual Patient? Patient { get; set; }

        /// <summary>
        /// Doctor who provided the service
        /// </summary>
        public virtual Doctor? Doctor { get; set; }

        // Helper Methods

        /// <summary>
        /// Checks if the bill has been paid
        /// </summary>
        public bool IsPaid => Status?.Equals("Paid", StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>
        /// Checks if the bill is overdue (more than 30 days)
        /// </summary>
        public bool IsOverdue => !IsPaid && (DateTime.UtcNow - BillDate).TotalDays > 30;

        /// <summary>
        /// Calculates the total bill amount with all charges, discounts, and taxes
        /// </summary>
        public decimal CalculateTotalAmount()
        {
            var subtotal = (ConsultationCharges ?? 0) +
                          (MedicationCharges ?? 0) +
                          (LabCharges ?? 0) +
                          (OtherCharges ?? 0);

            var discountedAmount = subtotal - (Discount ?? 0);
            var totalAmount = discountedAmount + (Tax ?? 0);

            return totalAmount;
        }
    }
}
