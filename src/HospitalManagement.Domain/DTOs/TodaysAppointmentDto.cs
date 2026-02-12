using System;

namespace HospitalManagement.Domain.DTOs
{
    public class TodaysAppointmentDto
    {
        public int AppointID { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime Date { get; set; }
        public string BillAmount { get; set; }
        public string BillStatus { get; set; }
        public string Disease { get; set; }
        public string Progress { get; set; }
        public string Prescription { get; set; }
        public int AppointmentStatus { get; set; }
    }
}
