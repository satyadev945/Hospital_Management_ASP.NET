using Xunit;
using Moq;
using AutoMapper;
using HospitalManagement.Application.Services;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services.Tests;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<DepartmentService>> _mockLogger;
    private readonly DepartmentService _service;

    public DepartmentServiceTests()
    {
        _mockRepository = new Mock<IDepartmentRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<DepartmentService>>();
        _service = new DepartmentService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void DepartmentService_Constructor_InitializesCorrectly()
    {
        // Arrange & Act & Assert
        Assert.NotNull(_service);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfDepartments()
    {
        // Arrange
        var departments = new List<Department>
        {
            new Department { DeptNo = 1, DeptName = "Cardiology" },
            new Department { DeptNo = 2, DeptName = "Neurology" }
        };
        var departmentDtos = new List<DepartmentDto>
        {
            new DepartmentDto { DeptNo = 1, DeptName = "Cardiology" },
            new DepartmentDto { DeptNo = 2, DeptName = "Neurology" }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(departments);
        _mockMapper.Setup(m => m.Map<IEnumerable<DepartmentDto>>(departments))
            .Returns(departmentDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDepartmentDto()
    {
        // Arrange
        var department = new Department { DeptNo = 1, DeptName = "Cardiology" };
        var departmentDto = new DepartmentDto { DeptNo = 1, DeptName = "Cardiology" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(department);
        _mockMapper.Setup(m => m.Map<DepartmentDto>(department))
            .Returns(departmentDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.DeptNo);
        Assert.Equal("Cardiology", result.DeptName);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Department?)null);

        // Act
        var result = await _service.GetByIdAsync(99999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedDepartment()
    {
        // Arrange
        var createDto = new DepartmentCreateDto
        {
            DeptName = "New Department",
            Description = "New Description"
        };
        var department = new Department { DeptName = "New Department" };
        var createdDepartment = new Department { DeptNo = 1, DeptName = "New Department" };
        var resultDto = new DepartmentDto { DeptNo = 1, DeptName = "New Department" };

        _mockMapper.Setup(m => m.Map<Department>(createDto))
            .Returns(department);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdDepartment);
        _mockMapper.Setup(m => m.Map<DepartmentDto>(createdDepartment))
            .Returns(resultDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.DeptNo);
        Assert.Equal("New Department", result.DeptName);
    }

    [Fact]
    public async Task UpdateAsync_ValidDto_UpdatesSuccessfully()
    {
        // Arrange
        var updateDto = new DepartmentUpdateDto { DeptName = "Updated Name" };
        var existingDepartment = new Department { DeptNo = 1, DeptName = "Original Name" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingDepartment);
        _mockMapper.Setup(m => m.Map(updateDto, existingDepartment))
            .Returns(existingDepartment);
        _mockRepository.Setup(r => r.UpdateAsync(existingDepartment, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Department>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsException()
    {
        // Arrange
        var updateDto = new DepartmentUpdateDto { DeptName = "Updated Name" };
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Department?)null);

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
}
