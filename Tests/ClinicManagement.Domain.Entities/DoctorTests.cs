using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Tests.ClinicManagement.Domain.Entities;

public class DoctorTests
{
    [Fact]
    public void Doctor_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Equal(0, doctor.Id);
        Assert.Equal(0, doctor.UserId);
        Assert.Null(doctor.User);
        Assert.Equal(string.Empty, doctor.Specialization);
        Assert.Equal(string.Empty, doctor.Qualification);
        Assert.Equal(0, doctor.Experience);
        Assert.Null(doctor.Description);
        Assert.Equal(default(DateTime), doctor.CreatedDate);
        Assert.Null(doctor.ModifiedDate);
        Assert.False(doctor.IsActive);
        Assert.Equal("System", doctor.CreatedBy);
        Assert.Null(doctor.ModifiedBy);
        Assert.NotNull(doctor.Appointments);
        Assert.Empty(doctor.Appointments);
    }

    [Fact]
    public void Doctor_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var doctor = new Doctor();
        var now = DateTime.UtcNow;

        // Act
        doctor.Id = 1;
        doctor.UserId = 100;
        doctor.Specialization = "Cardiology";
        doctor.Qualification = "MBBS, MD";
        doctor.Experience = 10;
        doctor.Description = "Experienced cardiologist";
        doctor.CreatedDate = now;
        doctor.ModifiedDate = now;
        doctor.IsActive = true;
        doctor.CreatedBy = "Admin";
        doctor.ModifiedBy = "Admin";

        // Assert
        Assert.Equal(1, doctor.Id);
        Assert.Equal(100, doctor.UserId);
        Assert.Equal("Cardiology", doctor.Specialization);
        Assert.Equal("MBBS, MD", doctor.Qualification);
        Assert.Equal(10, doctor.Experience);
        Assert.Equal("Experienced cardiologist", doctor.Description);
        Assert.Equal(now, doctor.CreatedDate);
        Assert.Equal(now, doctor.ModifiedDate);
        Assert.True(doctor.IsActive);
        Assert.Equal("Admin", doctor.CreatedBy);
        Assert.Equal("Admin", doctor.ModifiedBy);
    }

    [Fact]
    public void Doctor_UserNavigationProperty_ShouldBeSettable()
    {
        // Arrange
        var doctor = new Doctor();
        var user = new User { Id = 100, Name = "Dr. Smith" };

        // Act
        doctor.User = user;

        // Assert
        Assert.NotNull(doctor.User);
        Assert.Equal(100, doctor.User.Id);
        Assert.Equal("Dr. Smith", doctor.User.Name);
    }

    [Fact]
    public void Doctor_AppointmentsCollection_ShouldBeModifiable()
    {
        // Arrange
        var doctor = new Doctor();
        var appointment = new Appointment { Id = 1, DoctorId = doctor.Id };

        // Act
        doctor.Appointments.Add(appointment);

        // Assert
        Assert.Single(doctor.Appointments);
        Assert.Contains(appointment, doctor.Appointments);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(30)]
    public void Doctor_Experience_ShouldAcceptValidValues(int experience)
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Experience = experience;

        // Assert
        Assert.Equal(experience, doctor.Experience);
    }

    [Fact]
    public void Doctor_IsActive_ShouldToggle()
    {
        // Arrange
        var doctor = new Doctor { IsActive = false };

        // Act
        doctor.IsActive = true;

        // Assert
        Assert.True(doctor.IsActive);
    }

    [Fact]
    public void Doctor_Description_CanBeNull()
    {
        // Arrange
        var doctor = new Doctor { Description = null };

        // Assert
        Assert.Null(doctor.Description);
    }

    [Fact]
    public void Doctor_ModifiedDate_CanBeNull()
    {
        // Arrange
        var doctor = new Doctor { ModifiedDate = null };

        // Assert
        Assert.Null(doctor.ModifiedDate);
    }

    [Fact]
    public void Doctor_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Equal("System", doctor.CreatedBy);
    }
}
