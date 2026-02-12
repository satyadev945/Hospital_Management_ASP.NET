using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class DoctorTests
{
    [Fact]
    public void Doctor_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Equal(string.Empty, doctor.Name);
        Assert.Equal(string.Empty, doctor.Email);
        Assert.Equal(string.Empty, doctor.Password);
        Assert.Equal(string.Empty, doctor.Phone);
        Assert.Equal(string.Empty, doctor.Gender);
        Assert.Equal(string.Empty, doctor.Address);
        Assert.Equal(string.Empty, doctor.Specialization);
        Assert.Equal(string.Empty, doctor.Qualification);
        Assert.True(doctor.Status);
        Assert.Equal("System", doctor.CreatedBy);
        Assert.NotNull(doctor.Appointments);
    }

    [Fact]
    public void Doctor_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var birthDate = new DateTime(1980, 5, 15);

        // Act
        doctor.DoctorID = 1;
        doctor.Name = "Dr. Smith";
        doctor.Email = "smith@hospital.com";
        doctor.Password = "hashedPassword";
        doctor.BirthDate = birthDate;
        doctor.DeptNo = 10;
        doctor.Phone = "9876543210";
        doctor.Gender = "Male";
        doctor.Address = "456 Medical Ave";
        doctor.Experience = 10;
        doctor.Salary = 150000m;
        doctor.ChargesPerVisit = 500m;
        doctor.Specialization = "Cardiology";
        doctor.Qualification = "MBBS, MD";
        doctor.Status = true;
        doctor.ReputationIndex = 4.5m;
        doctor.PatientsTreated = 1000;

        // Assert
        Assert.Equal(1, doctor.DoctorID);
        Assert.Equal("Dr. Smith", doctor.Name);
        Assert.Equal("smith@hospital.com", doctor.Email);
        Assert.Equal("hashedPassword", doctor.Password);
        Assert.Equal(birthDate, doctor.BirthDate);
        Assert.Equal(10, doctor.DeptNo);
        Assert.Equal("9876543210", doctor.Phone);
        Assert.Equal("Male", doctor.Gender);
        Assert.Equal("456 Medical Ave", doctor.Address);
        Assert.Equal(10, doctor.Experience);
        Assert.Equal(150000m, doctor.Salary);
        Assert.Equal(500m, doctor.ChargesPerVisit);
        Assert.Equal("Cardiology", doctor.Specialization);
        Assert.Equal("MBBS, MD", doctor.Qualification);
        Assert.True(doctor.Status);
        Assert.Equal(4.5m, doctor.ReputationIndex);
        Assert.Equal(1000, doctor.PatientsTreated);
    }

    [Fact]
    public void Doctor_Status_DefaultsToTrue()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.True(doctor.Status);
    }

    [Fact]
    public void Doctor_Department_CanBeNull()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Null(doctor.Department);
    }

    [Fact]
    public void Doctor_Department_CanBeSet()
    {
        // Arrange
        var doctor = new Doctor();
        var department = new Department { DeptNo = 1, DeptName = "Cardiology" };

        // Act
        doctor.Department = department;

        // Assert
        Assert.NotNull(doctor.Department);
        Assert.Equal(1, doctor.Department.DeptNo);
    }

    [Fact]
    public void Doctor_Appointments_InitializesAsEmptyList()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.NotNull(doctor.Appointments);
        Assert.Empty(doctor.Appointments);
    }

    [Fact]
    public void Doctor_Appointments_CanAddItems()
    {
        // Arrange
        var doctor = new Doctor();
        var appointment = new Appointment { AppointmentID = 1 };

        // Act
        doctor.Appointments.Add(appointment);

        // Assert
        Assert.Single(doctor.Appointments);
        Assert.Contains(appointment, doctor.Appointments);
    }

    [Fact]
    public void Doctor_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Null(doctor.ModifiedDate);
    }

    [Fact]
    public void Doctor_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Null(doctor.ModifiedBy);
    }

    [Fact]
    public void Doctor_ReputationIndex_DefaultsToZero()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Equal(0m, doctor.ReputationIndex);
    }

    [Fact]
    public void Doctor_PatientsTreated_DefaultsToZero()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Equal(0, doctor.PatientsTreated);
    }
}
