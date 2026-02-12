using Xunit;
using Moq;
using AutoMapper;
using HospitalManagement.Application.Services;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services.Tests;

public class BillServiceTests
{
    private readonly Mock<IBillRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<BillService>> _mockLogger;
    private readonly BillService _service;

    public BillServiceTests()
    {
        _mockRepository = new Mock<IBillRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<BillService>>();
        _service = new BillService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public void BillService_Constructor_InitializesCorrectly()
    {
        // Arrange & Act & Assert
        Assert.NotNull(_service);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfBills()
    {
        // Arrange
        var bills = new List<Bill>
        {
            new Bill { BillID = 1, Amount = 1000m },
            new Bill { BillID = 2, Amount = 2000m }
        };
        var billDtos = new List<BillDto>
        {
            new BillDto { BillID = 1, Amount = 1000m },
            new BillDto { BillID = 2, Amount = 2000m }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(bills);
        _mockMapper.Setup(m => m.Map<IEnumerable<BillDto>>(bills))
            .Returns(billDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsBillDto()
    {
        // Arrange
        var bill = new Bill { BillID = 1, Amount = 1000m };
        var billDto = new BillDto { BillID = 1, Amount = 1000m };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bill);
        _mockMapper.Setup(m => m.Map<BillDto>(bill))
            .Returns(billDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.BillID);
        Assert.Equal(1000m, result.Amount);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Bill?)null);

        // Act
        var result = await _service.GetByIdAsync(99999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedBill()
    {
        // Arrange
        var createDto = new BillCreateDto
        {
            PatientID = 1,
            AppointmentID = 2,
            Amount = 5000m
        };
        var bill = new Bill { PatientID = 1, AppointmentID = 2, Amount = 5000m };
        var createdBill = new Bill { BillID = 1, PatientID = 1, AppointmentID = 2, Amount = 5000m };
        var resultDto = new BillDto { BillID = 1, PatientID = 1, AppointmentID = 2, Amount = 5000m };

        _mockMapper.Setup(m => m.Map<Bill>(createDto))
            .Returns(bill);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Bill>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdBill);
        _mockMapper.Setup(m => m.Map<BillDto>(createdBill))
            .Returns(resultDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.BillID);
        Assert.Equal(5000m, result.Amount);
    }

    [Fact]
    public async Task UpdateAsync_ValidDto_UpdatesSuccessfully()
    {
        // Arrange
        var updateDto = new BillUpdateDto { IsPaid = true };
        var existingBill = new Bill { BillID = 1, IsPaid = false };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBill);
        _mockMapper.Setup(m => m.Map(updateDto, existingBill))
            .Returns(existingBill);
        _mockRepository.Setup(r => r.UpdateAsync(existingBill, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(1, updateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Bill>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ThrowsException()
    {
        // Arrange
        var updateDto = new BillUpdateDto { IsPaid = true };
        _mockRepository.Setup(r => r.GetByIdAsync(99999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Bill?)null);

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
    public async Task GetByPatientIdAsync_ValidId_ReturnsBills()
    {
        // Arrange
        var bills = new List<Bill>
        {
            new Bill { BillID = 1, PatientID = 100, Amount = 1000m }
        };
        var billDtos = new List<BillDto>
        {
            new BillDto { BillID = 1, PatientID = 100, Amount = 1000m }
        };

        _mockRepository.Setup(r => r.GetByPatientIdAsync(100, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bills);
        _mockMapper.Setup(m => m.Map<IEnumerable<BillDto>>(bills))
            .Returns(billDtos);

        // Act
        var result = await _service.GetByPatientIdAsync(100);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
