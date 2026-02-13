using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Patient mappings
        CreateMap<Patient, PatientDto>()
            .ReverseMap();

        // Doctor mappings
        CreateMap<Doctor, DoctorDto>()
            .ReverseMap();

        // Department mappings
        CreateMap<Department, DepartmentDto>()
            .ReverseMap();

        // Appointment mappings
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.Name : null))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Name : null));

        CreateMap<AppointmentDto, Appointment>()
            .ForMember(dest => dest.Patient, opt => opt.Ignore())
            .ForMember(dest => dest.Doctor, opt => opt.Ignore());

        // Bill mappings
        CreateMap<Bill, BillDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Appointment != null && src.Appointment.Patient != null ? src.Appointment.Patient.Name : null));

        CreateMap<BillDto, Bill>()
            .ForMember(dest => dest.Appointment, opt => opt.Ignore());

        // Staff mappings
        CreateMap<Staff, StaffDto>();

        CreateMap<StaffDto, Staff>();
    }
}
