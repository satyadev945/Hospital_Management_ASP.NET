using System;

namespace HospitalManagement.Domain.DTOs
{
    public class PatientDto
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
    }
}
