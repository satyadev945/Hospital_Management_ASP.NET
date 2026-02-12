using Xunit;
using Moq;
using AutoMapper;
using HospitalManagement.Application.Services;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services.Tests;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<DoctorService>> _mockLogger;
    private readonly DoctorService _service;

    public DoctorServiceTests()
    {
        _mockRepository = new Mock<IDoctorRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<DoctorService>>();
        _service = new DoctorService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void DoctorService_Constructor_InitializesCorrectly()
    {
        // Arrange
        var mockRepository = new Mock<IDoctorRepository>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<DoctorService>>();

        // Act
        var service = new DoctorService(mockRepository.Object, mockMapper.Object, mockLogger.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfDoctors()
    {
        // Arrange
        var doctors = new List<Doctor>
        {
            new Doctor { DoctorID = 1, Name = "Dr. Smith" },
            new Doctor { DoctorID = 2, Name = "Dr. Jones" }
        };
        var doctorDtos = new List<DoctorDto>
        {
            new DoctorDto { DoctorID = 1, Name = "Dr. Smith" },
            new DoctorDto { DoctorID = 2, Name = "Dr. Jones" }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctors);
        _mockMapper.Setup(m => m.Map<IEnumerable<DoctorDto>>(doctors))
            .Returns(doctorDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDoctorDto()
    {
        // Arrange
        var doctor = new Doctor { DoctorID = 1, Name = "Dr. Smith" };
        var doctorDto = new DoctorDto { DoctorID = 1, Name = "Dr. Smith" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);
        _mockMapper.Setup(m => m.Map<DoctorDto>(doctor))
            .Returns(doctorDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.DoctorID);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        // Act
        var result = await _service.GetByIdAsync(99999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedDoctor()
    {
        // Arrange
        var createDto = new DoctorCreateDto
        {
            Name = "New Doctor",
            Email = "newdoctor@hospital.com",
            Password = "password"
        };
        var doctor = new Doctor { Name = "New Doctor", Email = "newdoctor@hospital.com" };
        var createdDoctor = new Doctor { DoctorID = 1, Name = "New Doctor", Email = "newdoctor@hospital.com" };
        var resultDto = new DoctorDto { DoctorID = 1, Name = "New Doctor", Email = "newdoctor@hospital.com" };

        _mockRepository.Setup(r => r.GetByEmailAsync(createDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);
        _mockMapper.Setup(m => m.Map<Doctor>(createDto))
            .Returns(doctor);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdDoctor);
        _mockMapper.Setup(m => m.Map<DoctorDto>(createdDoctor))
            .Returns(resultDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.DoctorID);
    }

    [Fact]
    public async Task CreateAsync_DuplicateEmail_ThrowsException()
    {
        // Arrange
        var createDto = new DoctorCreateDto
        {
            Name = "Duplicate",
            Email = "duplicate@hospital.com",
            Password = "password"
        };
        var existingDoctor = new Doctor { DoctorID = 1, Email = "duplicate@hospital.com" };

        _mockRepository.Setup(r => r.GetByEmailAsync(createDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingDoctor);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task UpdateAsync_ValidDto_UpdatesSuccessfully()
    {
        // Arrange
        var updateDto = new DoctorUpdateDto { Name = "Updated Name" };
        var existingDoctor = new Doctor { DoctorID = 1, Name = "Original Name" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingDoctor);
        _mockMapper.Setup(m => m.Map(updateDto, existingDoctor))
            .Returns(existingDoctor);
        _mockRepository.Setup(r => r.UpdateAsync(existingDoctor, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsException()
    {
        // Arrange
        var updateDto = new DoctorUpdateDto { Name = "Updated Name" };
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

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
    public async Task SearchAsync_ValidSearchTerm_ReturnsDoctors()
    {
        // Arrange
        var doctors = new List<Doctor>
        {
            new Doctor { DoctorID = 1, Name = "Dr. Search" }
        };
        var doctorDtos = new List<DoctorDto>
        {
            new DoctorDto { DoctorID = 1, Name = "Dr. Search" }
        };

        _mockRepository.Setup(r => r.SearchAsync("Search", It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctors);
        _mockMapper.Setup(m => m.Map<IEnumerable<DoctorDto>>(doctors))
            .Returns(doctorDtos);

        // Act
        var result = await _service.SearchAsync("Search");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByDepartmentAsync_ValidDeptNo_ReturnsDoctors()
    {
        // Arrange
        var doctors = new List<Doctor>
        {
            new Doctor { DoctorID = 1, Name = "Dr. Dept", DeptNo = 10 }
        };
        var doctorDtos = new List<DoctorDto>
        {
            new DoctorDto { DoctorID = 1, Name = "Dr. Dept", DeptNo = 10 }
        };

        _mockRepository.Setup(r => r.GetByDepartmentAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctors);
        _mockMapper.Setup(m => m.Map<IEnumerable<DoctorDto>>(doctors))
            .Returns(doctorDtos);

        // Act
        var result = await _service.GetByDepartmentAsync(10);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
