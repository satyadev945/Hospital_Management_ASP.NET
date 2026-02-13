using ClinicManagement.Application.Interfaces;
using ClinicManagement.Application.Services;
using ClinicManagement.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClinicManagement.UnitTests.Services;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly PatientService _patientService;

    public PatientServiceTests()
    {
        _mockPatientRepository = new Mock<IPatientRepository>();
        _patientService = new PatientService(_mockPatientRepository.Object);
    }

    [Fact]
    public async Task GetPatientByIdAsync_WhenPatientExists_ReturnsPatient()
    {
        // Arrange
        var patientId = 1;
        var expectedPatient = new Patient
        {
            Id = patientId,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Email = "john.doe@example.com",
            PhoneNumber = "555-0100"
        };

        _mockPatientRepository
            .Setup(repo => repo.GetByIdAsync(patientId))
            .ReturnsAsync(expectedPatient);

        // Act
        var result = await _patientService.GetPatientByIdAsync(patientId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedPatient);
        _mockPatientRepository.Verify(repo => repo.GetByIdAsync(patientId), Times.Once);
    }

    [Fact]
    public async Task GetPatientByIdAsync_WhenPatientDoesNotExist_ReturnsNull()
    {
        // Arrange
        var patientId = 999;
        _mockPatientRepository
            .Setup(repo => repo.GetByIdAsync(patientId))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _patientService.GetPatientByIdAsync(patientId);

        // Assert
        result.Should().BeNull();
        _mockPatientRepository.Verify(repo => repo.GetByIdAsync(patientId), Times.Once);
    }

    [Fact]
    public async Task GetAllPatientsAsync_ReturnsAllPatients()
    {
        // Arrange
        var expectedPatients = new List<Patient>
        {
            new Patient
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                PhoneNumber = "555-0100"
            },
            new Patient
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 15),
                Email = "jane.smith@example.com",
                PhoneNumber = "555-0101"
            }
        };

        _mockPatientRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedPatients);

        // Act
        var result = await _patientService.GetAllPatientsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedPatients);
        _mockPatientRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreatePatientAsync_WithValidPatient_ReturnsCreatedPatient()
    {
        // Arrange
        var newPatient = new Patient
        {
            FirstName = "Alice",
            LastName = "Johnson",
            DateOfBirth = new DateTime(1995, 3, 20),
            Email = "alice.johnson@example.com",
            PhoneNumber = "555-0102"
        };

        _mockPatientRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Patient>()))
            .ReturnsAsync((Patient p) => { p.Id = 3; return p; });

        // Act
        var result = await _patientService.CreatePatientAsync(newPatient);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(3);
        result.FirstName.Should().Be("Alice");
        result.LastName.Should().Be("Johnson");
        _mockPatientRepository.Verify(repo => repo.AddAsync(It.IsAny<Patient>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePatientAsync_WithValidPatient_ReturnsTrue()
    {
        // Arrange
        var existingPatient = new Patient
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Email = "john.doe@example.com",
            PhoneNumber = "555-0100"
        };

        _mockPatientRepository
            .Setup(repo => repo.UpdateAsync(It.IsAny<Patient>()))
            .ReturnsAsync(true);

        // Act
        var result = await _patientService.UpdatePatientAsync(existingPatient);

        // Assert
        result.Should().BeTrue();
        _mockPatientRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Patient>()), Times.Once);
    }

    [Fact]
    public async Task DeletePatientAsync_WithExistingPatientId_ReturnsTrue()
    {
        // Arrange
        var patientId = 1;
        _mockPatientRepository
            .Setup(repo => repo.DeleteAsync(patientId))
            .ReturnsAsync(true);

        // Act
        var result = await _patientService.DeletePatientAsync(patientId);

        // Assert
        result.Should().BeTrue();
        _mockPatientRepository.Verify(repo => repo.DeleteAsync(patientId), Times.Once);
    }

    [Fact]
    public async Task DeletePatientAsync_WithNonExistingPatientId_ReturnsFalse()
    {
        // Arrange
        var patientId = 999;
        _mockPatientRepository
            .Setup(repo => repo.DeleteAsync(patientId))
            .ReturnsAsync(false);

        // Act
        var result = await _patientService.DeletePatientAsync(patientId);

        // Assert
        result.Should().BeFalse();
        _mockPatientRepository.Verify(repo => repo.DeleteAsync(patientId), Times.Once);
    }
}
