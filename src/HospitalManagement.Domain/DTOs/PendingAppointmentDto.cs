using System;

namespace HospitalManagement.Domain.DTOs
{
    public class PendingAppointmentDto
    {
        public int AppointID { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime Date { get; set; }
        public int AppointmentStatus { get; set; }
    }
}
