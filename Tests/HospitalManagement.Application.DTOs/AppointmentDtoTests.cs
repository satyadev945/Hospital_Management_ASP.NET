using Xunit;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.DTOs.Tests;

public class AppointmentDtoTests
{
    [Fact]
    public void AppointmentDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new AppointmentDto();

        // Assert
        Assert.Equal(string.Empty, dto.PatientName);
        Assert.Equal(string.Empty, dto.DoctorName);
        Assert.Equal(string.Empty, dto.TimeSlot);
        Assert.Equal(string.Empty, dto.Status);
        Assert.False(dto.FeedbackGiven);
    }

    [Fact]
    public void AppointmentDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new AppointmentDto();
        var appointmentDate = new DateTime(2024, 12, 25, 10, 0, 0);

        // Act
        dto.AppointmentID = 1;
        dto.PatientID = 100;
        dto.PatientName = "John Doe";
        dto.DoctorID = 200;
        dto.DoctorName = "Dr. Smith";
        dto.AppointmentDate = appointmentDate;
        dto.TimeSlot = "10:00 AM - 11:00 AM";
        dto.Status = "Approved";
        dto.Disease = "Flu";
        dto.Progress = "Improving";
        dto.Prescription = "Rest and fluids";
        dto.FeedbackGiven = true;

        // Assert
        Assert.Equal(1, dto.AppointmentID);
        Assert.Equal(100, dto.PatientID);
        Assert.Equal("John Doe", dto.PatientName);
        Assert.Equal(200, dto.DoctorID);
        Assert.Equal("Dr. Smith", dto.DoctorName);
        Assert.Equal(appointmentDate, dto.AppointmentDate);
        Assert.Equal("10:00 AM - 11:00 AM", dto.TimeSlot);
        Assert.Equal("Approved", dto.Status);
        Assert.Equal("Flu", dto.Disease);
        Assert.Equal("Improving", dto.Progress);
        Assert.Equal("Rest and fluids", dto.Prescription);
        Assert.True(dto.FeedbackGiven);
    }

    [Fact]
    public void AppointmentDto_Disease_CanBeNull()
    {
        // Arrange & Act
        var dto = new AppointmentDto();

        // Assert
        Assert.Null(dto.Disease);
    }

    [Fact]
    public void AppointmentDto_Progress_CanBeNull()
    {
        // Arrange & Act
        var dto = new AppointmentDto();

        // Assert
        Assert.Null(dto.Progress);
    }

    [Fact]
    public void AppointmentDto_Prescription_CanBeNull()
    {
        // Arrange & Act
        var dto = new AppointmentDto();

        // Assert
        Assert.Null(dto.Prescription);
    }
}

public class AppointmentCreateDtoTests
{
    [Fact]
    public void AppointmentCreateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new AppointmentCreateDto();

        // Assert
        Assert.Equal(string.Empty, dto.TimeSlot);
    }

    [Fact]
    public void AppointmentCreateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new AppointmentCreateDto();
        var appointmentDate = new DateTime(2024, 12, 30, 14, 0, 0);

        // Act
        dto.PatientID = 100;
        dto.DoctorID = 200;
        dto.AppointmentDate = appointmentDate;
        dto.TimeSlot = "2:00 PM - 3:00 PM";

        // Assert
        Assert.Equal(100, dto.PatientID);
        Assert.Equal(200, dto.DoctorID);
        Assert.Equal(appointmentDate, dto.AppointmentDate);
        Assert.Equal("2:00 PM - 3:00 PM", dto.TimeSlot);
    }
}

public class AppointmentUpdateDtoTests
{
    [Fact]
    public void AppointmentUpdateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new AppointmentUpdateDto();

        // Assert
        Assert.Equal(string.Empty, dto.Status);
    }

    [Fact]
    public void AppointmentUpdateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new AppointmentUpdateDto();

        // Act
        dto.Status = "Completed";
        dto.Disease = "Cold";
        dto.Progress = "Recovered";
        dto.Prescription = "Vitamin C";

        // Assert
        Assert.Equal("Completed", dto.Status);
        Assert.Equal("Cold", dto.Disease);
        Assert.Equal("Recovered", dto.Progress);
        Assert.Equal("Vitamin C", dto.Prescription);
    }

    [Fact]
    public void AppointmentUpdateDto_Disease_CanBeNull()
    {
        // Arrange & Act
        var dto = new AppointmentUpdateDto();

        // Assert
        Assert.Null(dto.Disease);
    }

    [Fact]
    public void AppointmentUpdateDto_Progress_CanBeNull()
    {
        // Arrange & Act
        var dto = new AppointmentUpdateDto();

        // Assert
        Assert.Null(dto.Progress);
    }

    [Fact]
    public void AppointmentUpdateDto_Prescription_CanBeNull()
    {
        // Arrange & Act
        var dto = new AppointmentUpdateDto();

        // Assert
        Assert.Null(dto.Prescription);
    }
}
