using Xunit;
using Moq;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Tests;

public class AppointmentRepositoryTests
{
    private readonly Mock<HospitalDbContext> _mockContext;
    private readonly AppointmentRepository _repository;

    public AppointmentRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new HospitalDbContext(options);
        _repository = new AppointmentRepository(context);
    }

    [Fact]
    public void AppointmentRepository_Constructor_InitializesCorrectly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new HospitalDbContext(options);

        // Act
        var repository = new AppointmentRepository(context);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task AddAsync_ValidAppointment_ReturnsAppointment()
    {
        // Arrange
        var appointment = new Appointment
        {
            PatientID = 1,
            DoctorID = 2,
            AppointmentDate = DateTime.Now,
            TimeSlot = "10:00 AM",
            Status = "Pending"
        };

        // Act
        var result = await _repository.AddAsync(appointment);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.AppointmentID > 0);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsAppointment()
    {
        // Arrange
        var appointment = new Appointment
        {
            PatientID = 1,
            DoctorID = 2,
            AppointmentDate = DateTime.Now,
            TimeSlot = "10:00 AM"
        };
        var added = await _repository.AddAsync(appointment);

        // Act
        var result = await _repository.GetByIdAsync(added.AppointmentID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(added.AppointmentID, result.AppointmentID);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange & Act
        var result = await _repository.GetByIdAsync(99999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ExistingId_ReturnsTrue()
    {
        // Arrange
        var appointment = new Appointment
        {
            PatientID = 1,
            DoctorID = 2,
            AppointmentDate = DateTime.Now,
            TimeSlot = "10:00 AM"
        };
        var added = await _repository.AddAsync(appointment);

        // Act
        var result = await _repository.ExistsAsync(added.AppointmentID);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_NonExistingId_ReturnsFalse()
    {
        // Arrange & Act
        var result = await _repository.ExistsAsync(99999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ValidAppointment_UpdatesSuccessfully()
    {
        // Arrange
        var appointment = new Appointment
        {
            PatientID = 1,
            DoctorID = 2,
            AppointmentDate = DateTime.Now,
            TimeSlot = "10:00 AM",
            Status = "Pending"
        };
        var added = await _repository.AddAsync(appointment);
        added.Status = "Approved";

        // Act
        await _repository.UpdateAsync(added);
        var result = await _repository.GetByIdAsync(added.AppointmentID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Approved", result.Status);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_DeletesSuccessfully()
    {
        // Arrange
        var appointment = new Appointment
        {
            PatientID = 1,
            DoctorID = 2,
            AppointmentDate = DateTime.Now,
            TimeSlot = "10:00 AM"
        };
        var added = await _repository.AddAsync(appointment);

        // Act
        await _repository.DeleteAsync(added.AppointmentID);
        var result = await _repository.GetByIdAsync(added.AppointmentID);

        // Assert
        Assert.Null(result);
    }
}
