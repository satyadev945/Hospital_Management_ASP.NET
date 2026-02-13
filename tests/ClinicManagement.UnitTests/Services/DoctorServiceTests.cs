using ClinicManagement.Application.Interfaces;
using ClinicManagement.Application.Services;
using ClinicManagement.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClinicManagement.UnitTests.Services;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly DoctorService _doctorService;

    public DoctorServiceTests()
    {
        _mockDoctorRepository = new Mock<IDoctorRepository>();
        _doctorService = new DoctorService(_mockDoctorRepository.Object);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorExists_ReturnsDoctor()
    {
        // Arrange
        var doctorId = 1;
        var expectedDoctor = new Doctor
        {
            Id = doctorId,
            FirstName = "Sarah",
            LastName = "Williams",
            Specialization = "Cardiology",
            Email = "sarah.williams@clinic.com",
            PhoneNumber = "555-0200",
            LicenseNumber = "MD12345"
        };

        _mockDoctorRepository
            .Setup(repo => repo.GetByIdAsync(doctorId))
            .ReturnsAsync(expectedDoctor);

        // Act
        var result = await _doctorService.GetDoctorByIdAsync(doctorId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedDoctor);
        _mockDoctorRepository.Verify(repo => repo.GetByIdAsync(doctorId), Times.Once);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorDoesNotExist_ReturnsNull()
    {
        // Arrange
        var doctorId = 999;
        _mockDoctorRepository
            .Setup(repo => repo.GetByIdAsync(doctorId))
            .ReturnsAsync((Doctor?)null);

        // Act
        var result = await _doctorService.GetDoctorByIdAsync(doctorId);

        // Assert
        result.Should().BeNull();
        _mockDoctorRepository.Verify(repo => repo.GetByIdAsync(doctorId), Times.Once);
    }

    [Fact]
    public async Task GetAllDoctorsAsync_ReturnsAllDoctors()
    {
        // Arrange
        var expectedDoctors = new List<Doctor>
        {
            new Doctor
            {
                Id = 1,
                FirstName = "Sarah",
                LastName = "Williams",
                Specialization = "Cardiology",
                Email = "sarah.williams@clinic.com",
                PhoneNumber = "555-0200",
                LicenseNumber = "MD12345"
            },
            new Doctor
            {
                Id = 2,
                FirstName = "Michael",
                LastName = "Brown",
                Specialization = "Pediatrics",
                Email = "michael.brown@clinic.com",
                PhoneNumber = "555-0201",
                LicenseNumber = "MD67890"
            }
        };

        _mockDoctorRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedDoctors);

        // Act
        var result = await _doctorService.GetAllDoctorsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedDoctors);
        _mockDoctorRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetDoctorsBySpecializationAsync_ReturnsMatchingDoctors()
    {
        // Arrange
        var specialization = "Cardiology";
        var expectedDoctors = new List<Doctor>
        {
            new Doctor
            {
                Id = 1,
                FirstName = "Sarah",
                LastName = "Williams",
                Specialization = specialization,
                Email = "sarah.williams@clinic.com",
                PhoneNumber = "555-0200",
                LicenseNumber = "MD12345"
            }
        };

        _mockDoctorRepository
            .Setup(repo => repo.GetBySpecializationAsync(specialization))
            .ReturnsAsync(expectedDoctors);

        // Act
        var result = await _doctorService.GetDoctorsBySpecializationAsync(specialization);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().AllSatisfy(d => d.Specialization.Should().Be(specialization));
        _mockDoctorRepository.Verify(repo => repo.GetBySpecializationAsync(specialization), Times.Once);
    }

    [Fact]
    public async Task CreateDoctorAsync_WithValidDoctor_ReturnsCreatedDoctor()
    {
        // Arrange
        var newDoctor = new Doctor
        {
            FirstName = "Emily",
            LastName = "Davis",
            Specialization = "Neurology",
            Email = "emily.davis@clinic.com",
            PhoneNumber = "555-0202",
            LicenseNumber = "MD11111"
        };

        _mockDoctorRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Doctor>()))
            .ReturnsAsync((Doctor d) => { d.Id = 3; return d; });

        // Act
        var result = await _doctorService.CreateDoctorAsync(newDoctor);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(3);
        result.FirstName.Should().Be("Emily");
        result.Specialization.Should().Be("Neurology");
        _mockDoctorRepository.Verify(repo => repo.AddAsync(It.IsAny<Doctor>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDoctorAsync_WithValidDoctor_ReturnsTrue()
    {
        // Arrange
        var existingDoctor = new Doctor
        {
            Id = 1,
            FirstName = "Sarah",
            LastName = "Williams",
            Specialization = "Cardiology",
            Email = "sarah.williams@clinic.com",
            PhoneNumber = "555-0200",
            LicenseNumber = "MD12345"
        };

        _mockDoctorRepository
            .Setup(repo => repo.UpdateAsync(It.IsAny<Doctor>()))
            .ReturnsAsync(true);

        // Act
        var result = await _doctorService.UpdateDoctorAsync(existingDoctor);

        // Assert
        result.Should().BeTrue();
        _mockDoctorRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Doctor>()), Times.Once);
    }

    [Fact]
    public async Task DeleteDoctorAsync_WithExistingDoctorId_ReturnsTrue()
    {
        // Arrange
        var doctorId = 1;
        _mockDoctorRepository
            .Setup(repo => repo.DeleteAsync(doctorId))
            .ReturnsAsync(true);

        // Act
        var result = await _doctorService.DeleteDoctorAsync(doctorId);

        // Assert
        result.Should().BeTrue();
        _mockDoctorRepository.Verify(repo => repo.DeleteAsync(doctorId), Times.Once);
    }

    [Fact]
    public async Task DeleteDoctorAsync_WithNonExistingDoctorId_ReturnsFalse()
    {
        // Arrange
        var doctorId = 999;
        _mockDoctorRepository
            .Setup(repo => repo.DeleteAsync(doctorId))
            .ReturnsAsync(false);

        // Act
        var result = await _doctorService.DeleteDoctorAsync(doctorId);

        // Assert
        result.Should().BeFalse();
        _mockDoctorRepository.Verify(repo => repo.DeleteAsync(doctorId), Times.Once);
    }

    [Fact]
    public async Task GetDoctorWithAppointmentsAsync_WhenDoctorExists_ReturnsDoctorWithAppointments()
    {
        // Arrange
        var doctorId = 1;
        var expectedDoctor = new Doctor
        {
            Id = doctorId,
            FirstName = "Sarah",
            LastName = "Williams",
            Specialization = "Cardiology",
            Email = "sarah.williams@clinic.com",
            PhoneNumber = "555-0200",
            LicenseNumber = "MD12345",
            Appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    DoctorId = doctorId,
                    PatientId = 1,
                    AppointmentDate = DateTime.Now.AddDays(1),
                    Status = "Scheduled"
                }
            }
        };

        _mockDoctorRepository
            .Setup(repo => repo.GetWithAppointmentsAsync(doctorId))
            .ReturnsAsync(expectedDoctor);

        // Act
        var result = await _doctorService.GetDoctorWithAppointmentsAsync(doctorId);

        // Assert
        result.Should().NotBeNull();
        result!.Appointments.Should().HaveCount(1);
        _mockDoctorRepository.Verify(repo => repo.GetWithAppointmentsAsync(doctorId), Times.Once);
    }
}
