using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using System;

namespace Tests.ClinicManagement.Domain.Entities;

public class AppointmentTests
{
    [Fact]
    public void Appointment_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Equal(0, appointment.Id);
        Assert.Equal(0, appointment.PatientId);
        Assert.Null(appointment.Patient);
        Assert.Equal(0, appointment.DoctorId);
        Assert.Null(appointment.Doctor);
        Assert.Equal(default(DateTime), appointment.AppointmentDate);
        Assert.Equal(string.Empty, appointment.Reason);
        Assert.Equal(default(AppointmentStatus), appointment.Status);
        Assert.Null(appointment.Notes);
        Assert.Equal(default(DateTime), appointment.CreatedDate);
        Assert.Null(appointment.ModifiedDate);
        Assert.False(appointment.IsActive);
        Assert.Equal("System", appointment.CreatedBy);
        Assert.Null(appointment.ModifiedBy);
    }

    [Fact]
    public void Appointment_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var appointment = new Appointment();
        var now = DateTime.UtcNow;
        var appointmentDate = DateTime.UtcNow.AddDays(7);

        // Act
        appointment.Id = 1;
        appointment.PatientId = 10;
        appointment.DoctorId = 20;
        appointment.AppointmentDate = appointmentDate;
        appointment.Reason = "Regular checkup";
        appointment.Status = AppointmentStatus.Approved;
        appointment.Notes = "Patient has mild symptoms";
        appointment.CreatedDate = now;
        appointment.ModifiedDate = now;
        appointment.IsActive = true;
        appointment.CreatedBy = "Admin";
        appointment.ModifiedBy = "Doctor";

        // Assert
        Assert.Equal(1, appointment.Id);
        Assert.Equal(10, appointment.PatientId);
        Assert.Equal(20, appointment.DoctorId);
        Assert.Equal(appointmentDate, appointment.AppointmentDate);
        Assert.Equal("Regular checkup", appointment.Reason);
        Assert.Equal(AppointmentStatus.Approved, appointment.Status);
        Assert.Equal("Patient has mild symptoms", appointment.Notes);
        Assert.Equal(now, appointment.CreatedDate);
        Assert.Equal(now, appointment.ModifiedDate);
        Assert.True(appointment.IsActive);
        Assert.Equal("Admin", appointment.CreatedBy);
        Assert.Equal("Doctor", appointment.ModifiedBy);
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Approved)]
    [InlineData(AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.Cancelled)]
    public void Appointment_Status_ShouldAcceptAllValidStatuses(AppointmentStatus status)
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Status = status;

        // Assert
        Assert.Equal(status, appointment.Status);
    }

    [Fact]
    public void Appointment_PatientNavigationProperty_ShouldBeSettable()
    {
        // Arrange
        var appointment = new Appointment();
        var patient = new Patient { Id = 10, UserId = 100 };

        // Act
        appointment.Patient = patient;

        // Assert
        Assert.NotNull(appointment.Patient);
        Assert.Equal(10, appointment.Patient.Id);
    }

    [Fact]
    public void Appointment_DoctorNavigationProperty_ShouldBeSettable()
    {
        // Arrange
        var appointment = new Appointment();
        var doctor = new Doctor { Id = 20, UserId = 200 };

        // Act
        appointment.Doctor = doctor;

        // Assert
        Assert.NotNull(appointment.Doctor);
        Assert.Equal(20, appointment.Doctor.Id);
    }

    [Fact]
    public void Appointment_Notes_CanBeNull()
    {
        // Arrange
        var appointment = new Appointment { Notes = null };

        // Assert
        Assert.Null(appointment.Notes);
    }

    [Fact]
    public void Appointment_IsActive_ShouldToggle()
    {
        // Arrange
        var appointment = new Appointment { IsActive = false };

        // Act
        appointment.IsActive = true;

        // Assert
        Assert.True(appointment.IsActive);
    }

    [Fact]
    public void Appointment_AppointmentDate_ShouldAcceptFutureDate()
    {
        // Arrange
        var appointment = new Appointment();
        var futureDate = DateTime.UtcNow.AddDays(30);

        // Act
        appointment.AppointmentDate = futureDate;

        // Assert
        Assert.Equal(futureDate, appointment.AppointmentDate);
    }

    [Fact]
    public void Appointment_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Equal("System", appointment.CreatedBy);
    }

    [Fact]
    public void Appointment_ModifiedDate_CanBeNull()
    {
        // Arrange
        var appointment = new Appointment { ModifiedDate = null };

        // Assert
        Assert.Null(appointment.ModifiedDate);
    }
}
