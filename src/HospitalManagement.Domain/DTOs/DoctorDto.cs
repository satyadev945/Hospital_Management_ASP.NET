using System;

namespace HospitalManagement.Domain.DTOs
{
    public class DoctorDto
    {
        public int DoctorID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
        public int DeptNo { get; set; }
        public string DepartmentName { get; set; }
        public decimal ChargesPerVisit { get; set; }
        public decimal? MonthlySalary { get; set; }
        public decimal? ReputeIndex { get; set; }
        public int PatientsTreated { get; set; }
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public int? WorkExperience { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
    }
}
