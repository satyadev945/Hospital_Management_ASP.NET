using System.Collections.Generic;

namespace HospitalManagement.Domain.DTOs
{
    public class NotificationDto
    {
        public int NotificationType { get; set; }
        public string DoctorName { get; set; }
        public string Timings { get; set; }
        public string Message { get; set; }
        public int PatientID { get; set; }
        public int UnseenCount { get; set; }
        public IEnumerable<AppointmentDto> Appointments { get; set; }
    }
}
