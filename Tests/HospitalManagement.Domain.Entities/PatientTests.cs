using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class PatientTests
{
    [Fact]
    public void Patient_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.Equal(string.Empty, patient.Name);
        Assert.Equal(string.Empty, patient.Phone);
        Assert.Equal(string.Empty, patient.Address);
        Assert.Equal(string.Empty, patient.Gender);
        Assert.Equal(string.Empty, patient.Email);
        Assert.Equal(string.Empty, patient.Password);
        Assert.True(patient.Status);
        Assert.Equal("System", patient.CreatedBy);
        Assert.NotNull(patient.Appointments);
        Assert.NotNull(patient.Bills);
    }

    [Fact]
    public void Patient_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        patient.PatientID = 1;
        patient.Name = "John Doe";
        patient.Phone = "1234567890";
        patient.Address = "123 Main St";
        patient.BirthDate = birthDate;
        patient.Gender = "Male";
        patient.Email = "john@example.com";
        patient.Password = "password123";
        patient.Status = false;

        // Assert
        Assert.Equal(1, patient.PatientID);
        Assert.Equal("John Doe", patient.Name);
        Assert.Equal("1234567890", patient.Phone);
        Assert.Equal("123 Main St", patient.Address);
        Assert.Equal(birthDate, patient.BirthDate);
        Assert.Equal("Male", patient.Gender);
        Assert.Equal("john@example.com", patient.Email);
        Assert.Equal("password123", patient.Password);
        Assert.False(patient.Status);
    }

    [Fact]
    public void Patient_Status_DefaultsToTrue()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.True(patient.Status);
    }

    [Fact]
    public void Patient_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.Null(patient.ModifiedDate);
    }

    [Fact]
    public void Patient_ModifiedDate_CanBeSet()
    {
        // Arrange
        var patient = new Patient();
        var modifiedDate = DateTime.UtcNow;

        // Act
        patient.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, patient.ModifiedDate);
    }

    [Fact]
    public void Patient_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.Null(patient.ModifiedBy);
    }

    [Fact]
    public void Patient_Appointments_InitializesAsEmptyList()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Appointments);
        Assert.Empty(patient.Appointments);
    }

    [Fact]
    public void Patient_Bills_InitializesAsEmptyList()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Bills);
        Assert.Empty(patient.Bills);
    }

    [Fact]
    public void Patient_Appointments_CanAddItems()
    {
        // Arrange
        var patient = new Patient();
        var appointment = new Appointment { AppointmentID = 1 };

        // Act
        patient.Appointments.Add(appointment);

        // Assert
        Assert.Single(patient.Appointments);
        Assert.Contains(appointment, patient.Appointments);
    }

    [Fact]
    public void Patient_Bills_CanAddItems()
    {
        // Arrange
        var patient = new Patient();
        var bill = new Bill { BillID = 1 };

        // Act
        patient.Bills.Add(bill);

        // Assert
        Assert.Single(patient.Bills);
        Assert.Contains(bill, patient.Bills);
    }
}
