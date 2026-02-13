using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClinicManagement.IntegrationTests.Repositories;

public class PatientRepositoryTests : IDisposable
{
    private readonly ClinicDbContext _context;
    private readonly PatientRepository _repository;

    public PatientRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ClinicDbContext(options);
        _repository = new PatientRepository(_context);
    }

    [Fact]
    public async Task AddAsync_AddsPatientToDatabase()
    {
        // Arrange
        var patient = new Patient
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Email = "john.doe@example.com",
            PhoneNumber = "555-0100"
        };

        // Act
        var result = await _repository.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        
        var savedPatient = await _context.Patients.FindAsync(result.Id);
        savedPatient.Should().NotBeNull();
        savedPatient!.FirstName.Should().Be("John");
        savedPatient.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public async Task GetByIdAsync_WhenPatientExists_ReturnsPatient()
    {
        // Arrange
        var patient = new Patient
        {
            FirstName = "Jane",
            LastName = "Smith",
            DateOfBirth = new DateTime(1985, 5, 15),
            Email = "jane.smith@example.com",
            PhoneNumber = "555-0101"
        };
        
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(patient.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(patient.Id);
        result.FirstName.Should().Be("Jane");
        result.LastName.Should().Be("Smith");
    }

    [Fact]
    public async Task GetByIdAsync_WhenPatientDoesNotExist_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPatients()
    {
        // Arrange
        var patients = new List<Patient>
        {
            new Patient
            {
                FirstName = "Alice",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1995, 3, 20),
                Email = "alice.johnson@example.com",
                PhoneNumber = "555-0102"
            },
            new Patient
            {
                FirstName = "Bob",
                LastName = "Wilson",
                DateOfBirth = new DateTime(1988, 7, 10),
                Email = "bob.wilson@example.com",
                PhoneNumber = "555-0103"
            }
        };

        _context.Patients.AddRange(patients);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.FirstName == "Alice");
        result.Should().Contain(p => p.FirstName == "Bob");
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingPatient()
    {
        // Arrange
        var patient = new Patient
        {
            FirstName = "Charlie",
            LastName = "Brown",
            DateOfBirth = new DateTime(1992, 11, 5),
            Email = "charlie.brown@example.com",
            PhoneNumber = "555-0104"
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        // Modify patient
        patient.Email = "charlie.updated@example.com";
        patient.PhoneNumber = "555-9999";

        // Act
        var result = await _repository.UpdateAsync(patient);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().BeTrue();
        
        var updatedPatient = await _context.Patients.FindAsync(patient.Id);
        updatedPatient.Should().NotBeNull();
        updatedPatient!.Email.Should().Be("charlie.updated@example.com");
        updatedPatient.PhoneNumber.Should().Be("555-9999");
    }

    [Fact]
    public async Task DeleteAsync_WhenPatientExists_RemovesPatient()
    {
        // Arrange
        var patient = new Patient
        {
            FirstName = "David",
            LastName = "Miller",
            DateOfBirth = new DateTime(1987, 4, 25),
            Email = "david.miller@example.com",
            PhoneNumber = "555-0105"
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        var patientId = patient.Id;

        // Act
        var result = await _repository.DeleteAsync(patientId);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().BeTrue();
        
        var deletedPatient = await _context.Patients.FindAsync(patientId);
        deletedPatient.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenPatientDoesNotExist_ReturnsFalse()
    {
        // Act
        var result = await _repository.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetWithAppointmentsAsync_ReturnsPatientWithAppointments()
    {
        // Arrange
        var doctor = new Doctor
        {
            FirstName = "Dr. Sarah",
            LastName = "Williams",
            Specialization = "Cardiology",
            Email = "sarah.williams@clinic.com",
            PhoneNumber = "555-0200",
            LicenseNumber = "MD12345"
        };

        var patient = new Patient
        {
            FirstName = "Emma",
            LastName = "Davis",
            DateOfBirth = new DateTime(1993, 8, 12),
            Email = "emma.davis@example.com",
            PhoneNumber = "555-0106"
        };

        _context.Doctors.Add(doctor);
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        var appointment = new Appointment
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            AppointmentDate = DateTime.Now.AddDays(1),
            Status = "Scheduled",
            Reason = "Annual checkup"
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetWithAppointmentsAsync(patient.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(patient.Id);
        result.Appointments.Should().NotBeNull();
        result.Appointments.Should().HaveCount(1);
        result.Appointments.First().Reason.Should().Be("Annual checkup");
    }

    [Fact]
    public async Task GetByEmailAsync_WhenPatientExists_ReturnsPatient()
    {
        // Arrange
        var patient = new Patient
        {
            FirstName = "Frank",
            LastName = "Anderson",
            DateOfBirth = new DateTime(1991, 6, 18),
            Email = "frank.anderson@example.com",
            PhoneNumber = "555-0107"
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync("frank.anderson@example.com");

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("frank.anderson@example.com");
        result.FirstName.Should().Be("Frank");
    }

    [Fact]
    public async Task GetByEmailAsync_WhenPatientDoesNotExist_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByEmailAsync("nonexistent@example.com");

        // Assert
        result.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
