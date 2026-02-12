using System;

namespace HospitalManagement.Domain.DTOs
{
    public class StaffDto
    {
        public int StaffID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Designation { get; set; }
        public char Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string HighestQualification { get; set; }
        public decimal? Salary { get; set; }
    }
}
