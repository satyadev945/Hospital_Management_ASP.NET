using Xunit;
using AutoMapper;
using HospitalManagement.Application.Mappings;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Mappings.Tests;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void MappingProfile_Configuration_IsValid()
    {
        // Arrange & Act & Assert
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void MappingProfile_Patient_ToPatientDto_MapsCorrectly()
    {
        // Arrange
        var patient = new Patient
        {
            PatientID = 1,
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "1234567890",
            BirthDate = new DateTime(1990, 1, 1),
            Gender = "Male",
            Address = "123 Main St"
        };

        // Act
        var dto = _mapper.Map<PatientDto>(patient);

        // Assert
        Assert.Equal(patient.PatientID, dto.PatientID);
        Assert.Equal(patient.Name, dto.Name);
        Assert.Equal(patient.Email, dto.Email);
        Assert.Equal(patient.Phone, dto.Phone);
        Assert.Equal(patient.BirthDate, dto.BirthDate);
        Assert.Equal(patient.Gender, dto.Gender);
        Assert.Equal(patient.Address, dto.Address);
    }

    [Fact]
    public void MappingProfile_PatientCreateDto_ToPatient_MapsCorrectly()
    {
        // Arrange
        var dto = new PatientCreateDto
        {
            Name = "Jane Doe",
            Email = "jane@example.com",
            Password = "password",
            Phone = "9876543210",
            BirthDate = new DateTime(1995, 5, 15),
            Gender = "Female",
            Address = "456 Oak Ave"
        };

        // Act
        var patient = _mapper.Map<Patient>(dto);

        // Assert
        Assert.Equal(dto.Name, patient.Name);
        Assert.Equal(dto.Email, patient.Email);
        Assert.Equal(dto.Phone, patient.Phone);
        Assert.Equal(dto.BirthDate, patient.BirthDate);
        Assert.Equal(dto.Gender, patient.Gender);
        Assert.Equal(dto.Address, patient.Address);
        Assert.True(patient.Status);
    }

    [Fact]
    public void MappingProfile_Doctor_ToDoctorDto_MapsCorrectly()
    {
        // Arrange
        var doctor = new Doctor
        {
            DoctorID = 1,
            Name = "Dr. Smith",
            Email = "smith@hospital.com",
            Phone = "5551234567",
            DeptNo = 10,
            Specialization = "Cardiology",
            Department = new Department { DeptNo = 10, DeptName = "Cardiology" }
        };

        // Act
        var dto = _mapper.Map<DoctorDto>(doctor);

        // Assert
        Assert.Equal(doctor.DoctorID, dto.DoctorID);
        Assert.Equal(doctor.Name, dto.Name);
        Assert.Equal(doctor.Email, dto.Email);
        Assert.Equal(doctor.Phone, dto.Phone);
        Assert.Equal(doctor.DeptNo, dto.DeptNo);
        Assert.Equal(doctor.Specialization, dto.Specialization);
        Assert.Equal("Cardiology", dto.DepartmentName);
    }

    [Fact]
    public void MappingProfile_DoctorCreateDto_ToDoctor_MapsCorrectly()
    {
        // Arrange
        var dto = new DoctorCreateDto
        {
            Name = "Dr. Johnson",
            Email = "johnson@hospital.com",
            Password = "password",
            Phone = "5559876543",
            DeptNo = 5,
            Specialization = "Neurology",
            Qualification = "MBBS, MD",
            Experience = 10,
            Salary = 150000m,
            ChargesPerVisit = 500m
        };

        // Act
        var doctor = _mapper.Map<Doctor>(dto);

        // Assert
        Assert.Equal(dto.Name, doctor.Name);
        Assert.Equal(dto.Email, doctor.Email);
        Assert.Equal(dto.Phone, doctor.Phone);
        Assert.Equal(dto.DeptNo, doctor.DeptNo);
        Assert.Equal(dto.Specialization, doctor.Specialization);
        Assert.Equal(dto.Qualification, doctor.Qualification);
        Assert.Equal(dto.Experience, doctor.Experience);
        Assert.Equal(dto.Salary, doctor.Salary);
        Assert.Equal(dto.ChargesPerVisit, doctor.ChargesPerVisit);
        Assert.True(doctor.Status);
        Assert.Equal(0m, doctor.ReputationIndex);
        Assert.Equal(0, doctor.PatientsTreated);
    }

    [Fact]
    public void MappingProfile_Department_ToDepartmentDto_MapsCorrectly()
    {
        // Arrange
        var department = new Department
        {
            DeptNo = 1,
            DeptName = "Cardiology",
            Description = "Heart care"
        };

        // Act
        var dto = _mapper.Map<DepartmentDto>(department);

        // Assert
        Assert.Equal(department.DeptNo, dto.DeptNo);
        Assert.Equal(department.DeptName, dto.DeptName);
        Assert.Equal(department.Description, dto.Description);
    }

    [Fact]
    public void MappingProfile_DepartmentCreateDto_ToDepartment_MapsCorrectly()
    {
        // Arrange
        var dto = new DepartmentCreateDto
        {
            DeptName = "Neurology",
            Description = "Brain care"
        };

        // Act
        var department = _mapper.Map<Department>(dto);

        // Assert
        Assert.Equal(dto.DeptName, department.DeptName);
        Assert.Equal(dto.Description, department.Description);
        Assert.True(department.IsActive);
    }

    [Fact]
    public void MappingProfile_Appointment_ToAppointmentDto_MapsCorrectly()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentID = 1,
            PatientID = 100,
            DoctorID = 200,
            AppointmentDate = new DateTime(2024, 12, 25),
            TimeSlot = "10:00 AM",
            Status = "Approved",
            Patient = new Patient { Name = "John Doe" },
            Doctor = new Doctor { Name = "Dr. Smith" }
        };

        // Act
        var dto = _mapper.Map<AppointmentDto>(appointment);

        // Assert
        Assert.Equal(appointment.AppointmentID, dto.AppointmentID);
        Assert.Equal(appointment.PatientID, dto.PatientID);
        Assert.Equal(appointment.DoctorID, dto.DoctorID);
        Assert.Equal(appointment.AppointmentDate, dto.AppointmentDate);
        Assert.Equal(appointment.TimeSlot, dto.TimeSlot);
        Assert.Equal(appointment.Status, dto.Status);
        Assert.Equal("John Doe", dto.PatientName);
        Assert.Equal("Dr. Smith", dto.DoctorName);
    }

    [Fact]
    public void MappingProfile_AppointmentCreateDto_ToAppointment_MapsCorrectly()
    {
        // Arrange
        var dto = new AppointmentCreateDto
        {
            PatientID = 100,
            DoctorID = 200,
            AppointmentDate = new DateTime(2024, 12, 30),
            TimeSlot = "2:00 PM"
        };

        // Act
        var appointment = _mapper.Map<Appointment>(dto);

        // Assert
        Assert.Equal(dto.PatientID, appointment.PatientID);
        Assert.Equal(dto.DoctorID, appointment.DoctorID);
        Assert.Equal(dto.AppointmentDate, appointment.AppointmentDate);
        Assert.Equal(dto.TimeSlot, appointment.TimeSlot);
        Assert.Equal("Pending", appointment.Status);
        Assert.False(appointment.FeedbackGiven);
    }

    [Fact]
    public void MappingProfile_Bill_ToBillDto_MapsCorrectly()
    {
        // Arrange
        var bill = new Bill
        {
            BillID = 1,
            PatientID = 100,
            AppointmentID = 200,
            Amount = 5000m,
            IsPaid = true,
            BillDate = new DateTime(2024, 12, 25),
            Patient = new Patient { Name = "John Doe" }
        };

        // Act
        var dto = _mapper.Map<BillDto>(bill);

        // Assert
        Assert.Equal(bill.BillID, dto.BillID);
        Assert.Equal(bill.PatientID, dto.PatientID);
        Assert.Equal(bill.AppointmentID, dto.AppointmentID);
        Assert.Equal(bill.Amount, dto.Amount);
        Assert.Equal(bill.IsPaid, dto.IsPaid);
        Assert.Equal(bill.BillDate, dto.BillDate);
        Assert.Equal("John Doe", dto.PatientName);
    }

    [Fact]
    public void MappingProfile_BillCreateDto_ToBill_MapsCorrectly()
    {
        // Arrange
        var dto = new BillCreateDto
        {
            PatientID = 100,
            AppointmentID = 200,
            Amount = 7500m
        };

        // Act
        var bill = _mapper.Map<Bill>(dto);

        // Assert
        Assert.Equal(dto.PatientID, bill.PatientID);
        Assert.Equal(dto.AppointmentID, bill.AppointmentID);
        Assert.Equal(dto.Amount, bill.Amount);
        Assert.False(bill.IsPaid);
    }

    [Fact]
    public void MappingProfile_OtherStaff_ToOtherStaffDto_MapsCorrectly()
    {
        // Arrange
        var staff = new OtherStaff
        {
            StaffID = 1,
            Name = "Jane Smith",
            Phone = "5551234567",
            Designation = "Nurse",
            Salary = 50000m
        };

        // Act
        var dto = _mapper.Map<OtherStaffDto>(staff);

        // Assert
        Assert.Equal(staff.StaffID, dto.StaffID);
        Assert.Equal(staff.Name, dto.Name);
        Assert.Equal(staff.Phone, dto.Phone);
        Assert.Equal(staff.Designation, dto.Designation);
        Assert.Equal(staff.Salary, dto.Salary);
    }

    [Fact]
    public void MappingProfile_OtherStaffCreateDto_ToOtherStaff_MapsCorrectly()
    {
        // Arrange
        var dto = new OtherStaffCreateDto
        {
            Name = "Bob Johnson",
            Phone = "5559876543",
            Designation = "Technician",
            Salary = 45000m
        };

        // Act
        var staff = _mapper.Map<OtherStaff>(dto);

        // Assert
        Assert.Equal(dto.Name, staff.Name);
        Assert.Equal(dto.Phone, staff.Phone);
        Assert.Equal(dto.Designation, staff.Designation);
        Assert.Equal(dto.Salary, staff.Salary);
        Assert.True(staff.Status);
    }
}
