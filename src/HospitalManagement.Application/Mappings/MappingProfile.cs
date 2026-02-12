using AutoMapper;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile for mapping between domain entities and DTOs
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ConfigureDepartmentMappings();
            ConfigureDoctorMappings();
            ConfigurePatientMappings();
            ConfigureOtherStaffMappings();
            ConfigureAppointmentMappings();
            ConfigureBillMappings();
            ConfigureTreatmentHistoryMappings();
            ConfigureFeedbackMappings();
        }

        /// <summary>
        /// Configure mappings for Department entity
        /// </summary>
        private void ConfigureDepartmentMappings()
        {
            // Department to DepartmentDto
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.NumberOfDoctors,
                    opt => opt.MapFrom(src => src.Doctors != null ? src.Doctors.Count : 0));

            // DepartmentCreateDto to Department
            CreateMap<DepartmentCreateDto, Department>()
                .ForMember(dest => dest.Doctors, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

            // DepartmentUpdateDto to Department
            CreateMap<DepartmentUpdateDto, Department>()
                .ForMember(dest => dest.Doctors, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

        /// <summary>
        /// Configure mappings for Doctor entity
        /// </summary>
        private void ConfigureDoctorMappings()
        {
            // Doctor to DoctorDto
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => ConvertGenderToChar(src.Gender)))
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department != null ? src.Department.DeptName : string.Empty));

            // DoctorCreateDto to Doctor
            CreateMap<DoctorCreateDto, Doctor>()
                .ForMember(dest => dest.DoctorID, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.PatientsTreated, opt => opt.Ignore())
                .ForMember(dest => dest.ReputeIndex, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.TreatmentHistories, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

            // DoctorUpdateDto to Doctor
            CreateMap<DoctorUpdateDto, Doctor>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.HasValue ? src.Gender.Value.ToString() : null))
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.PatientsTreated, opt => opt.Ignore())
                .ForMember(dest => dest.ReputeIndex, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.TreatmentHistories, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

        /// <summary>
        /// Configure mappings for Patient entity
        /// </summary>
        private void ConfigurePatientMappings()
        {
            // Patient to PatientDto
            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => ConvertGenderToChar(src.Gender)));

            // PatientCreateDto to Patient
            CreateMap<PatientCreateDto, Patient>()
                .ForMember(dest => dest.PatientID, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.Bills, opt => opt.Ignore())
                .ForMember(dest => dest.TreatmentHistories, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

            // PatientUpdateDto to Patient
            CreateMap<PatientUpdateDto, Patient>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.HasValue ? src.Gender.Value.ToString() : null))
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Appointments, opt => opt.Ignore())
                .ForMember(dest => dest.Bills, opt => opt.Ignore())
                .ForMember(dest => dest.TreatmentHistories, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

        /// <summary>
        /// Configure mappings for OtherStaff entity
        /// </summary>
        private void ConfigureOtherStaffMappings()
        {
            // OtherStaff to StaffDto
            CreateMap<OtherStaff, StaffDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => ConvertGenderToChar(src.Gender)));

            // StaffCreateDto to OtherStaff
            CreateMap<StaffCreateDto, OtherStaff>()
                .ForMember(dest => dest.StaffID, opt => opt.Ignore())
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.EmergencyContact, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyContactName, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Shift, opt => opt.Ignore())
                .ForMember(dest => dest.EmploymentType, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeID, opt => opt.Ignore())
                .ForMember(dest => dest.NationalID, opt => opt.Ignore())
                .ForMember(dest => dest.BloodGroup, opt => opt.Ignore())
                .ForMember(dest => dest.TerminationDate, opt => opt.Ignore())
                .ForMember(dest => dest.TerminationReason, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

            // StaffUpdateDto to OtherStaff
            CreateMap<StaffUpdateDto, OtherStaff>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.HasValue ? src.Gender.Value.ToString() : null))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyContact, opt => opt.Ignore())
                .ForMember(dest => dest.EmergencyContactName, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Shift, opt => opt.Ignore())
                .ForMember(dest => dest.EmploymentType, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeID, opt => opt.Ignore())
                .ForMember(dest => dest.NationalID, opt => opt.Ignore())
                .ForMember(dest => dest.BloodGroup, opt => opt.Ignore())
                .ForMember(dest => dest.TerminationDate, opt => opt.Ignore())
                .ForMember(dest => dest.TerminationReason, opt => opt.Ignore())
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

        /// <summary>
        /// Configure mappings for Appointment entity
        /// </summary>
        private void ConfigureAppointmentMappings()
        {
            // Appointment to AppointmentDto
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty))
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Name : string.Empty))
                .ForMember(dest => dest.AppointmentStatusText,
                    opt => opt.MapFrom(src => GetAppointmentStatusText(src.AppointmentStatus)));

            // AppointmentCreateDto to Appointment
            CreateMap<AppointmentCreateDto, Appointment>()
                .ForMember(dest => dest.AppointID, opt => opt.Ignore())
                .ForMember(dest => dest.AppointmentStatus, opt => opt.MapFrom(src => 2)) // Default to Pending
                .ForMember(dest => dest.BillAmount, opt => opt.Ignore())
                .ForMember(dest => dest.BillStatus, opt => opt.MapFrom(src => "Unpaid"))
                .ForMember(dest => dest.DoctorNotification, opt => opt.MapFrom(src => 2)) // Unseen
                .ForMember(dest => dest.PatientNotification, opt => opt.MapFrom(src => 2)) // Unseen
                .ForMember(dest => dest.FeedbackStatus, opt => opt.MapFrom(src => 2)) // Pending
                .ForMember(dest => dest.Disease, opt => opt.Ignore())
                .ForMember(dest => dest.Progress, opt => opt.Ignore())
                .ForMember(dest => dest.Prescription, opt => opt.Ignore())
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Bill, opt => opt.Ignore())
                .ForMember(dest => dest.TreatmentHistory, opt => opt.Ignore())
                .ForMember(dest => dest.Feedback, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

            // AppointmentUpdateDto to Appointment
            CreateMap<AppointmentUpdateDto, Appointment>()
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.Bill, opt => opt.Ignore())
                .ForMember(dest => dest.TreatmentHistory, opt => opt.Ignore())
                .ForMember(dest => dest.Feedback, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Appointment to PendingAppointmentDto
            CreateMap<Appointment, PendingAppointmentDto>()
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty))
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Name : string.Empty));

            // Appointment to TodaysAppointmentDto
            CreateMap<Appointment, TodaysAppointmentDto>()
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty))
                .ForMember(dest => dest.PatientName,
                    opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Name : string.Empty));
        }

        /// <summary>
        /// Configure mappings for Bill entity
        /// </summary>
        private void ConfigureBillMappings()
        {
            // Bill to BillHistoryDto
            CreateMap<Bill, BillHistoryDto>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.BillDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty))
                .ForMember(dest => dest.BillAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.BillStatus, opt => opt.MapFrom(src => src.Status));

            // Appointment to BillHistoryDto (for appointments with bill information)
            CreateMap<Appointment, BillHistoryDto>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.HasValue ? src.Date.Value.ToString("yyyy-MM-dd") : string.Empty))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty))
                .ForMember(dest => dest.BillAmount, opt => opt.MapFrom(src => src.BillAmount ?? 0))
                .ForMember(dest => dest.BillStatus, opt => opt.MapFrom(src => src.BillStatus ?? "Unpaid"));
        }

        /// <summary>
        /// Configure mappings for TreatmentHistory entity
        /// </summary>
        private void ConfigureTreatmentHistoryMappings()
        {
            // TreatmentHistory to TreatmentHistoryDto
            CreateMap<TreatmentHistory, TreatmentHistoryDto>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.TreatmentDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty))
                .ForMember(dest => dest.DiagnosedDisease, opt => opt.MapFrom(src => src.Disease ?? string.Empty))
                .ForMember(dest => dest.ProgressMade, opt => opt.MapFrom(src => src.Progress ?? string.Empty))
                .ForMember(dest => dest.Prescription, opt => opt.MapFrom(src => src.Prescription ?? string.Empty));

            // Appointment to TreatmentHistoryDto (for appointments with treatment information)
            CreateMap<Appointment, TreatmentHistoryDto>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.Date.HasValue ? src.Date.Value.ToString("yyyy-MM-dd") : string.Empty))
                .ForMember(dest => dest.DoctorName,
                    opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : string.Empty))
                .ForMember(dest => dest.DiagnosedDisease, opt => opt.MapFrom(src => src.Disease ?? string.Empty))
                .ForMember(dest => dest.ProgressMade, opt => opt.MapFrom(src => src.Progress ?? string.Empty))
                .ForMember(dest => dest.Prescription, opt => opt.MapFrom(src => src.Prescription ?? string.Empty));
        }

        /// <summary>
        /// Configure mappings for Feedback entity
        /// </summary>
        private void ConfigureFeedbackMappings()
        {
            // Basic Feedback entity mapping (can be extended as needed)
            CreateMap<Feedback, Feedback>()
                .ForMember(dest => dest.FeedbackID, opt => opt.Ignore());
        }

        /// <summary>
        /// Helper method to convert gender string to char for DTO
        /// </summary>
        private char ConvertGenderToChar(string gender)
        {
            if (string.IsNullOrEmpty(gender))
                return ' ';

            return gender[0];
        }

        /// <summary>
        /// Helper method to get appointment status text
        /// </summary>
        private string GetAppointmentStatusText(int? status)
        {
            return status switch
            {
                1 => "Approved",
                2 => "Pending",
                3 => "Completed",
                4 => "Rejected",
                _ => "Unknown"
            };
        }
    }
}
