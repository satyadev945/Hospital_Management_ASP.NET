using Xunit;
using Moq;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories.Tests;

public class PatientRepositoryTests
{
    private readonly PatientRepository _repository;
    private readonly HospitalDbContext _context;
    private readonly Mock<ILogger<PatientRepository>> _mockLogger;

    public PatientRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HospitalDbContext(options);
        _mockLogger = new Mock<ILogger<PatientRepository>>();
        _repository = new PatientRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public void PatientRepository_Constructor_InitializesCorrectly()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new HospitalDbContext(options);
        var logger = new Mock<ILogger<PatientRepository>>();

        // Act
        var repository = new PatientRepository(context, logger.Object);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task AddAsync_ValidPatient_ReturnsPatient()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "1234567890",
            BirthDate = new DateTime(1990, 1, 1),
            Gender = "Male",
            Address = "123 Main St",
            Status = true
        };

        // Act
        var result = await _repository.AddAsync(patient);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.PatientID > 0);
        Assert.Equal("John Doe", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsPatient()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "Jane Doe",
            Email = "jane@example.com",
            Phone = "9876543210",
            Status = true
        };
        var added = await _repository.AddAsync(patient);

        // Act
        var result = await _repository.GetByIdAsync(added.PatientID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(added.PatientID, result.PatientID);
        Assert.Equal("Jane Doe", result.Name);
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
    public async Task GetByEmailAsync_ExistingEmail_ReturnsPatient()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "Test User",
            Email = "test@example.com",
            Phone = "1111111111",
            Status = true
        };
        await _repository.AddAsync(patient);

        // Act
        var result = await _repository.GetByEmailAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_NonExistingEmail_ReturnsNull()
    {
        // Arrange & Act
        var result = await _repository.GetByEmailAsync("nonexistent@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ExistingId_ReturnsTrue()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "Exists Test",
            Email = "exists@example.com",
            Phone = "2222222222",
            Status = true
        };
        var added = await _repository.AddAsync(patient);

        // Act
        var result = await _repository.ExistsAsync(added.PatientID);

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
    public async Task UpdateAsync_ValidPatient_UpdatesSuccessfully()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "Original Name",
            Email = "original@example.com",
            Phone = "3333333333",
            Status = true
        };
        var added = await _repository.AddAsync(patient);
        added.Name = "Updated Name";

        // Act
        await _repository.UpdateAsync(added);
        var result = await _repository.GetByIdAsync(added.PatientID);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Name", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_SetsStatusToFalse()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "Delete Test",
            Email = "delete@example.com",
            Phone = "4444444444",
            Status = true
        };
        var added = await _repository.AddAsync(patient);

        // Act
        await _repository.DeleteAsync(added.PatientID);
        var result = await _repository.GetByIdAsync(added.PatientID);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Status);
    }

    [Fact]
    public async Task SearchAsync_MatchingName_ReturnsPatients()
    {
        // Arrange
        var patient1 = new Patient
        {
            Name = "Search Test One",
            Email = "search1@example.com",
            Phone = "5555555555",
            Status = true
        };
        var patient2 = new Patient
        {
            Name = "Search Test Two",
            Email = "search2@example.com",
            Phone = "6666666666",
            Status = true
        };
        await _repository.AddAsync(patient1);
        await _repository.AddAsync(patient2);

        // Act
        var results = await _repository.SearchAsync("Search Test");

        // Assert
        Assert.NotNull(results);
        Assert.True(results.Count() >= 2);
    }
}
