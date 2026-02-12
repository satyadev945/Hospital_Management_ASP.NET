using Xunit;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.DTOs.Tests;

public class DoctorDtoTests
{
    [Fact]
    public void DoctorDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new DoctorDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.DepartmentName);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(string.Empty, dto.Specialization);
        Assert.Equal(string.Empty, dto.Qualification);
    }

    [Fact]
    public void DoctorDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new DoctorDto();
        var birthDate = new DateTime(1980, 3, 15);

        // Act
        dto.DoctorID = 1;
        dto.Name = "Dr. Smith";
        dto.Email = "smith@hospital.com";
        dto.BirthDate = birthDate;
        dto.DeptNo = 10;
        dto.DepartmentName = "Cardiology";
        dto.Phone = "9876543210";
        dto.Gender = "Male";
        dto.Address = "123 Medical Blvd";
        dto.Experience = 15;
        dto.ChargesPerVisit = 500m;
        dto.Specialization = "Heart Surgeon";
        dto.Qualification = "MBBS, MD";
        dto.ReputationIndex = 4.8m;
        dto.PatientsTreated = 2000;

        // Assert
        Assert.Equal(1, dto.DoctorID);
        Assert.Equal("Dr. Smith", dto.Name);
        Assert.Equal("smith@hospital.com", dto.Email);
        Assert.Equal(birthDate, dto.BirthDate);
        Assert.Equal(10, dto.DeptNo);
        Assert.Equal("Cardiology", dto.DepartmentName);
        Assert.Equal("9876543210", dto.Phone);
        Assert.Equal("Male", dto.Gender);
        Assert.Equal("123 Medical Blvd", dto.Address);
        Assert.Equal(15, dto.Experience);
        Assert.Equal(500m, dto.ChargesPerVisit);
        Assert.Equal("Heart Surgeon", dto.Specialization);
        Assert.Equal("MBBS, MD", dto.Qualification);
        Assert.Equal(4.8m, dto.ReputationIndex);
        Assert.Equal(2000, dto.PatientsTreated);
    }

    [Fact]
    public void DoctorDto_Age_CalculatesCorrectly()
    {
        // Arrange
        var dto = new DoctorDto();
        var birthDate = DateTime.Now.AddYears(-40);

        // Act
        dto.BirthDate = birthDate;

        // Assert
        Assert.Equal(40, dto.Age);
    }
}

public class DoctorCreateDtoTests
{
    [Fact]
    public void DoctorCreateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new DoctorCreateDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.Password);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(string.Empty, dto.Specialization);
        Assert.Equal(string.Empty, dto.Qualification);
    }

    [Fact]
    public void DoctorCreateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new DoctorCreateDto();
        var birthDate = new DateTime(1975, 8, 20);

        // Act
        dto.Name = "Dr. Johnson";
        dto.Email = "johnson@hospital.com";
        dto.Password = "securePassword";
        dto.BirthDate = birthDate;
        dto.DeptNo = 5;
        dto.Phone = "5551234567";
        dto.Gender = "Female";
        dto.Address = "456 Health St";
        dto.Experience = 20;
        dto.Salary = 200000m;
        dto.ChargesPerVisit = 750m;
        dto.Specialization = "Neurologist";
        dto.Qualification = "MBBS, DNB";

        // Assert
        Assert.Equal("Dr. Johnson", dto.Name);
        Assert.Equal("johnson@hospital.com", dto.Email);
        Assert.Equal("securePassword", dto.Password);
        Assert.Equal(birthDate, dto.BirthDate);
        Assert.Equal(5, dto.DeptNo);
        Assert.Equal("5551234567", dto.Phone);
        Assert.Equal("Female", dto.Gender);
        Assert.Equal("456 Health St", dto.Address);
        Assert.Equal(20, dto.Experience);
        Assert.Equal(200000m, dto.Salary);
        Assert.Equal(750m, dto.ChargesPerVisit);
        Assert.Equal("Neurologist", dto.Specialization);
        Assert.Equal("MBBS, DNB", dto.Qualification);
    }
}

public class DoctorUpdateDtoTests
{
    [Fact]
    public void DoctorUpdateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new DoctorUpdateDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(string.Empty, dto.Specialization);
        Assert.Equal(string.Empty, dto.Qualification);
    }

    [Fact]
    public void DoctorUpdateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new DoctorUpdateDto();

        // Act
        dto.Name = "Dr. Brown";
        dto.Phone = "5559876543";
        dto.Address = "789 Care Ln";
        dto.Experience = 12;
        dto.Salary = 180000m;
        dto.ChargesPerVisit = 600m;
        dto.Specialization = "Orthopedic";
        dto.Qualification = "MBBS, MS";

        // Assert
        Assert.Equal("Dr. Brown", dto.Name);
        Assert.Equal("5559876543", dto.Phone);
        Assert.Equal("789 Care Ln", dto.Address);
        Assert.Equal(12, dto.Experience);
        Assert.Equal(180000m, dto.Salary);
        Assert.Equal(600m, dto.ChargesPerVisit);
        Assert.Equal("Orthopedic", dto.Specialization);
        Assert.Equal("MBBS, MS", dto.Qualification);
    }
}
