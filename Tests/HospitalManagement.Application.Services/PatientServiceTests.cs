using Xunit;
using Moq;
using AutoMapper;
using HospitalManagement.Application.Services;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services.Tests;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<PatientService>> _mockLogger;
    private readonly PatientService _service;

    public PatientServiceTests()
    {
        _mockRepository = new Mock<IPatientRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PatientService>>();
        _service = new PatientService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void PatientService_Constructor_InitializesCorrectly()
    {
        // Arrange
        var mockRepository = new Mock<IPatientRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<PatientService>>();

        // Act
        var service = new PatientService(mockRepository.Object, mockMapper.Object, mockLogger.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfPatients()
    {
        // Arrange
        var patients = new List<Patient>
        {
            new Patient { PatientID = 1, Name = "Patient 1" },
            new Patient { PatientID = 2, Name = "Patient 2" }
        };
        var patientDtos = new List<PatientDto>
        {
            new PatientDto { PatientID = 1, Name = "Patient 1" },
            new PatientDto { PatientID = 2, Name = "Patient 2" }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(patients);
        _mockMapper.Setup(m => m.Map<IEnumerable<PatientDto>>(patients))
            .Returns(patientDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsPatientDto()
    {
        // Arrange
        var patient = new Patient { PatientID = 1, Name = "John Doe" };
        var patientDto = new PatientDto { PatientID = 1, Name = "John Doe" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);
        _mockMapper.Setup(m => m.Map<PatientDto>(patient))
            .Returns(patientDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.PatientID);
        Assert.Equal("John Doe", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _service.GetByIdAsync(99999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedPatient()
    {
        // Arrange
        var createDto = new PatientCreateDto
        {
            Name = "New Patient",
            Email = "new@example.com",
            Password = "password"
        };
        var patient = new Patient { Name = "New Patient", Email = "new@example.com" };
        var createdPatient = new Patient { PatientID = 1, Name = "New Patient", Email = "new@example.com" };
        var resultDto = new PatientDto { PatientID = 1, Name = "New Patient", Email = "new@example.com" };

        _mockRepository.Setup(r => r.GetByEmailAsync(createDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);
        _mockMapper.Setup(m => m.Map<Patient>(createDto))
            .Returns(patient);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdPatient);
        _mockMapper.Setup(m => m.Map<PatientDto>(createdPatient))
            .Returns(resultDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.PatientID);
        Assert.Equal("New Patient", result.Name);
    }

    [Fact]
    public async Task CreateAsync_DuplicateEmail_ThrowsException()
    {
        // Arrange
        var createDto = new PatientCreateDto
        {
            Name = "Duplicate",
            Email = "duplicate@example.com",
            Password = "password"
        };
        var existingPatient = new Patient { PatientID = 1, Email = "duplicate@example.com" };

        _mockRepository.Setup(r => r.GetByEmailAsync(createDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPatient);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_ValidDto_UpdatesSuccessfully()
    {
        // Arrange
        var updateDto = new PatientUpdateDto { Name = "Updated Name" };
        var existingPatient = new Patient { PatientID = 1, Name = "Original Name" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPatient);
        _mockMapper.Setup(m => m.Map(updateDto, existingPatient))
            .Returns(existingPatient);
        _mockRepository.Setup(r => r.UpdateAsync(existingPatient, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsException()
    {
        // Arrange
        var updateDto = new PatientUpdateDto { Name = "Updated Name" };
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(99999, updateDto));
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_DeletesSuccessfully()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ThrowsException()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(99999));
    }

    [Fact]
    public async Task SearchAsync_ValidSearchTerm_ReturnsPatients()
    {
        // Arrange
        var patients = new List<Patient>
        {
            new Patient { PatientID = 1, Name = "Search Test" }
        };
        var patientDtos = new List<PatientDto>
        {
            new PatientDto { PatientID = 1, Name = "Search Test" }
        };

        _mockRepository.Setup(r => r.SearchAsync("Search", It.IsAny<CancellationToken>()))
            .ReturnsAsync(patients);
        _mockMapper.Setup(m => m.Map<IEnumerable<PatientDto>>(patients))
            .Returns(patientDtos);

        // Act
        var result = await _service.SearchAsync("Search");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
