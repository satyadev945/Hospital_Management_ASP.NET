using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class AppointmentTests
{
    [Fact]
    public void Appointment_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Equal(string.Empty, appointment.TimeSlot);
        Assert.Equal("Pending", appointment.Status);
        Assert.False(appointment.FeedbackGiven);
        Assert.Equal("System", appointment.CreatedBy);
    }

    [Fact]
    public void Appointment_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var appointmentDate = new DateTime(2024, 12, 25, 10, 0, 0);

        // Act
        appointment.AppointmentID = 1;
        appointment.PatientID = 100;
        appointment.DoctorID = 200;
        appointment.AppointmentDate = appointmentDate;
        appointment.TimeSlot = "10:00 AM - 11:00 AM";
        appointment.Status = "Approved";
        appointment.Disease = "Flu";
        appointment.Progress = "Improving";
        appointment.Prescription = "Paracetamol 500mg";
        appointment.FeedbackGiven = true;

        // Assert
        Assert.Equal(1, appointment.AppointmentID);
        Assert.Equal(100, appointment.PatientID);
        Assert.Equal(200, appointment.DoctorID);
        Assert.Equal(appointmentDate, appointment.AppointmentDate);
        Assert.Equal("10:00 AM - 11:00 AM", appointment.TimeSlot);
        Assert.Equal("Approved", appointment.Status);
        Assert.Equal("Flu", appointment.Disease);
        Assert.Equal("Improving", appointment.Progress);
        Assert.Equal("Paracetamol 500mg", appointment.Prescription);
        Assert.True(appointment.FeedbackGiven);
    }

    [Fact]
    public void Appointment_Status_DefaultsToPending()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Equal("Pending", appointment.Status);
    }

    [Fact]
    public void Appointment_FeedbackGiven_DefaultsToFalse()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.False(appointment.FeedbackGiven);
    }

    [Fact]
    public void Appointment_Disease_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.Disease);
    }

    [Fact]
    public void Appointment_Progress_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.Progress);
    }

    [Fact]
    public void Appointment_Prescription_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.Prescription);
    }

    [Fact]
    public void Appointment_Patient_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.Patient);
    }

    [Fact]
    public void Appointment_Doctor_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.Doctor);
    }

    [Fact]
    public void Appointment_Bill_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.Bill);
    }

    [Fact]
    public void Appointment_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.ModifiedDate);
    }

    [Fact]
    public void Appointment_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.ModifiedBy);
    }
}
