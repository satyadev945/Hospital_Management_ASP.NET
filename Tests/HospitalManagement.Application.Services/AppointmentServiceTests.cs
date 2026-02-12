using Xunit;
using Moq;
using AutoMapper;
using HospitalManagement.Application.Services;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services.Tests;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<AppointmentService>> _mockLogger;
    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _mockRepository = new Mock<IAppointmentRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<AppointmentService>>();
        _service = new AppointmentService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void AppointmentService_Constructor_InitializesCorrectly()
    {
        // Arrange & Act & Assert
        Assert.NotNull(_service);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment { AppointmentID = 1 },
            new Appointment { AppointmentID = 2 }
        };
        var appointmentDtos = new List<AppointmentDto>
        {
            new AppointmentDto { AppointmentID = 1 },
            new AppointmentDto { AppointmentID = 2 }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointments);
        _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsAppointmentDto()
    {
        // Arrange
        var appointment = new Appointment { AppointmentID = 1 };
        var appointmentDto = new AppointmentDto { AppointmentID = 1 };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);
        _mockMapper.Setup(m => m.Map<AppointmentDto>(appointment))
            .Returns(appointmentDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.AppointmentID);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment?)null);

        // Act
        var result = await _service.GetByIdAsync(99999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedAppointment()
    {
        // Arrange
        var createDto = new AppointmentCreateDto
        {
            PatientID = 1,
            DoctorID = 2,
            AppointmentDate = DateTime.Now,
            TimeSlot = "10:00 AM"
        };
        var appointment = new Appointment { PatientID = 1, DoctorID = 2 };
        var createdAppointment = new Appointment { AppointmentID = 1, PatientID = 1, DoctorID = 2 };
        var resultDto = new AppointmentDto { AppointmentID = 1, PatientID = 1, DoctorID = 2 };

        _mockMapper.Setup(m => m.Map<Appointment>(createDto))
            .Returns(appointment);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdAppointment);
        _mockMapper.Setup(m => m.Map<AppointmentDto>(createdAppointment))
            .Returns(resultDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.AppointmentID);
    }

    [Fact]
    public async Task UpdateAsync_ValidDto_UpdatesSuccessfully()
    {
        // Arrange
        var updateDto = new AppointmentUpdateDto { Status = "Approved" };
        var existingAppointment = new Appointment { AppointmentID = 1, Status = "Pending" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAppointment);
        _mockMapper.Setup(m => m.Map(updateDto, existingAppointment))
            .Returns(existingAppointment);
        _mockRepository.Setup(r => r.UpdateAsync(existingAppointment, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsException()
    {
        // Arrange
        var updateDto = new AppointmentUpdateDto { Status = "Approved" };
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(99999, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_DeletesSuccessfully()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ValidId_ReturnsAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment { AppointmentID = 1, PatientID = 100 }
        };
        var appointmentDtos = new List<AppointmentDto>
        {
            new AppointmentDto { AppointmentID = 1, PatientID = 100 }
        };

        _mockRepository.Setup(r => r.GetByPatientIdAsync(100, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointments);
        _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        // Act
        var result = await _service.GetByPatientIdAsync(100);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByDoctorIdAsync_ValidId_ReturnsAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment { AppointmentID = 1, DoctorID = 200 }
        };
        var appointmentDtos = new List<AppointmentDto>
        {
            new AppointmentDto { AppointmentID = 1, DoctorID = 200 }
        };

        _mockRepository.Setup(r => r.GetByDoctorIdAsync(200, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointments);
        _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(appointments))
            .Returns(appointmentDtos);

        // Act
        var result = await _service.GetByDoctorIdAsync(200);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task ApproveAppointmentAsync_ExistingId_ApprovesSuccessfully()
    {
        // Arrange
        var appointment = new Appointment { AppointmentID = 1, Status = "Pending" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);
        _mockRepository.Setup(r => r.UpdateAsync(appointment, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.ApproveAppointmentAsync(1);

        // Assert
        Assert.Equal("Approved", appointment.Status);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ApproveAppointmentAsync_NonExistingId_ThrowsException()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ApproveAppointmentAsync(99999));
    }
}
