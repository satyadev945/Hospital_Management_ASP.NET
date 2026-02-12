namespace HospitalManagement.Domain.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public decimal MonthlyIncome { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalStaff { get; set; }
    }
}
