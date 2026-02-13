namespace ClinicManagement.Application.DTOs;

public class AppointmentUpdateDto
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ReasonForVisit { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
