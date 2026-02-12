using Xunit;
using Moq;
using AutoMapper;
using HospitalManagement.Application.Services;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services.Tests;

public class OtherStaffServiceTests
{
    private readonly Mock<IOtherStaffRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<OtherStaffService>> _mockLogger;
    private readonly OtherStaffService _service;

    public OtherStaffServiceTests()
    {
        _mockRepository = new Mock<IOtherStaffRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<OtherStaffService>>();
        _service = new OtherStaffService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void OtherStaffService_Constructor_InitializesCorrectly()
    {
        // Arrange & Act & Assert
        Assert.NotNull(_service);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfStaff()
    {
        // Arrange
        var staff = new List<OtherStaff>
        {
            new OtherStaff { StaffID = 1, Name = "Staff 1" },
            new OtherStaff { StaffID = 2, Name = "Staff 2" }
        };
        var staffDtos = new List<OtherStaffDto>
        {
            new OtherStaffDto { StaffID = 1, Name = "Staff 1" },
            new OtherStaffDto { StaffID = 2, Name = "Staff 2" }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(staff);
        _mockMapper.Setup(m => m.Map<IEnumerable<OtherStaffDto>>(staff))
            .Returns(staffDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsStaffDto()
    {
        // Arrange
        var staff = new OtherStaff { StaffID = 1, Name = "Jane Doe" };
        var staffDto = new OtherStaffDto { StaffID = 1, Name = "Jane Doe" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(staff);
        _mockMapper.Setup(m => m.Map<OtherStaffDto>(staff))
            .Returns(staffDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.StaffID);
        Assert.Equal("Jane Doe", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OtherStaff?)null);

        // Act
        var result = await _service.GetByIdAsync(99999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedStaff()
    {
        // Arrange
        var createDto = new OtherStaffCreateDto
        {
            Name = "New Staff",
            Designation = "Nurse"
        };
        var staff = new OtherStaff { Name = "New Staff" };
        var createdStaff = new OtherStaff { StaffID = 1, Name = "New Staff" };
        var resultDto = new OtherStaffDto { StaffID = 1, Name = "New Staff" };

        _mockMapper.Setup(m => m.Map<OtherStaff>(createDto))
            .Returns(staff);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<OtherStaff>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdStaff);
        _mockMapper.Setup(m => m.Map<OtherStaffDto>(createdStaff))
            .Returns(resultDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.StaffID);
    }

    [Fact]
    public async Task UpdateAsync_ValidDto_UpdatesSuccessfully()
    {
        // Arrange
        var updateDto = new OtherStaffUpdateDto { Name = "Updated Name" };
        var existingStaff = new OtherStaff { StaffID = 1, Name = "Original Name" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStaff);
        _mockMapper.Setup(m => m.Map(updateDto, existingStaff))
            .Returns(existingStaff);
        _mockRepository.Setup(r => r.UpdateAsync(existingStaff, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<OtherStaff>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsException()
    {
        // Arrange
        var updateDto = new OtherStaffUpdateDto { Name = "Updated Name" };
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OtherStaff?)null);

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
    public async Task SearchAsync_ValidSearchTerm_ReturnsStaff()
    {
        // Arrange
        var staff = new List<OtherStaff>
        {
            new OtherStaff { StaffID = 1, Name = "Search Staff" }
        };
        var staffDtos = new List<OtherStaffDto>
        {
            new OtherStaffDto { StaffID = 1, Name = "Search Staff" }
        };

        _mockRepository.Setup(r => r.SearchAsync("Search", It.IsAny<CancellationToken>()))
            .ReturnsAsync(staff);
        _mockMapper.Setup(m => m.Map<IEnumerable<OtherStaffDto>>(staff))
            .Returns(staffDtos);

        // Act
        var result = await _service.SearchAsync("Search");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
