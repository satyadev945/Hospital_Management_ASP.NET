using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Tests.ClinicManagement.Domain.Entities;

public class PatientTests
{
    [Fact]
    public void Patient_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.Equal(0, patient.Id);
        Assert.Equal(0, patient.UserId);
        Assert.Null(patient.User);
        Assert.Null(patient.MedicalHistory);
        Assert.Equal(default(DateTime), patient.CreatedDate);
        Assert.Null(patient.ModifiedDate);
        Assert.False(patient.IsActive);
        Assert.Equal("System", patient.CreatedBy);
        Assert.Null(patient.ModifiedBy);
        Assert.NotNull(patient.Appointments);
        Assert.Empty(patient.Appointments);
    }

    [Fact]
    public void Patient_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var patient = new Patient();
        var now = DateTime.UtcNow;

        // Act
        patient.Id = 1;
        patient.UserId = 100;
        patient.MedicalHistory = "Diabetes, Hypertension";
        patient.CreatedDate = now;
        patient.ModifiedDate = now;
        patient.IsActive = true;
        patient.CreatedBy = "Admin";
        patient.ModifiedBy = "Doctor";

        // Assert
        Assert.Equal(1, patient.Id);
        Assert.Equal(100, patient.UserId);
        Assert.Equal("Diabetes, Hypertension", patient.MedicalHistory);
        Assert.Equal(now, patient.CreatedDate);
        Assert.Equal(now, patient.ModifiedDate);
        Assert.True(patient.IsActive);
        Assert.Equal("Admin", patient.CreatedBy);
        Assert.Equal("Doctor", patient.ModifiedBy);
    }

    [Fact]
    public void Patient_UserNavigationProperty_ShouldBeSettable()
    {
        // Arrange
        var patient = new Patient();
        var user = new User { Id = 100, Name = "John Doe" };

        // Act
        patient.User = user;

        // Assert
        Assert.NotNull(patient.User);
        Assert.Equal(100, patient.User.Id);
        Assert.Equal("John Doe", patient.User.Name);
    }

    [Fact]
    public void Patient_AppointmentsCollection_ShouldBeModifiable()
    {
        // Arrange
        var patient = new Patient();
        var appointment = new Appointment { Id = 1, PatientId = patient.Id };

        // Act
        patient.Appointments.Add(appointment);

        // Assert
        Assert.Single(patient.Appointments);
        Assert.Contains(appointment, patient.Appointments);
    }

    [Fact]
    public void Patient_MedicalHistory_CanBeNull()
    {
        // Arrange
        var patient = new Patient { MedicalHistory = null };

        // Assert
        Assert.Null(patient.MedicalHistory);
    }

    [Fact]
    public void Patient_IsActive_ShouldToggle()
    {
        // Arrange
        var patient = new Patient { IsActive = false };

        // Act
        patient.IsActive = true;

        // Assert
        Assert.True(patient.IsActive);
    }

    [Fact]
    public void Patient_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.Equal("System", patient.CreatedBy);
    }

    [Fact]
    public void Patient_ModifiedDate_CanBeNull()
    {
        // Arrange
        var patient = new Patient { ModifiedDate = null };

        // Assert
        Assert.Null(patient.ModifiedDate);
    }

    [Fact]
    public void Patient_MedicalHistory_ShouldAcceptLongStrings()
    {
        // Arrange
        var patient = new Patient();
        var longHistory = new string('A', 1000);

        // Act
        patient.MedicalHistory = longHistory;

        // Assert
        Assert.Equal(longHistory, patient.MedicalHistory);
    }

    [Fact]
    public void Patient_AppointmentsCollection_ShouldInitializeEmpty()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Appointments);
        Assert.Empty(patient.Appointments);
        Assert.IsAssignableFrom<ICollection<Appointment>>(patient.Appointments);
    }
}
