using System;

namespace HospitalManagement.Domain.DTOs
{
    public class AppointmentSlotDto
    {
        public string FreeSlot { get; set; }
        public int SlotHour { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public DateTime SlotTime { get; set; }
        public int Hour { get; set; }
        public bool IsAvailable { get; set; }
    }
}
