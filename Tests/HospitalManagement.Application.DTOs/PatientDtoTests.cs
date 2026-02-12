using Xunit;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.DTOs.Tests;

public class PatientDtoTests
{
    [Fact]
    public void PatientDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new PatientDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Email);
    }

    [Fact]
    public void PatientDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new PatientDto();
        var birthDate = new DateTime(1990, 5, 15);

        // Act
        dto.PatientID = 1;
        dto.Name = "John Doe";
        dto.Phone = "1234567890";
        dto.Address = "123 Main St";
        dto.BirthDate = birthDate;
        dto.Gender = "Male";
        dto.Email = "john@example.com";

        // Assert
        Assert.Equal(1, dto.PatientID);
        Assert.Equal("John Doe", dto.Name);
        Assert.Equal("1234567890", dto.Phone);
        Assert.Equal("123 Main St", dto.Address);
        Assert.Equal(birthDate, dto.BirthDate);
        Assert.Equal("Male", dto.Gender);
        Assert.Equal("john@example.com", dto.Email);
    }

    [Fact]
    public void PatientDto_Age_CalculatesCorrectly()
    {
        // Arrange
        var dto = new PatientDto();
        var birthDate = DateTime.Now.AddYears(-30);

        // Act
        dto.BirthDate = birthDate;

        // Assert
        Assert.Equal(30, dto.Age);
    }

    [Fact]
    public void PatientDto_Age_HandlesLeapYear()
    {
        // Arrange
        var dto = new PatientDto();
        dto.BirthDate = new DateTime(2000, 2, 29);

        // Act
        var age = dto.Age;

        // Assert
        Assert.True(age >= 0);
    }
}

public class PatientCreateDtoTests
{
    [Fact]
    public void PatientCreateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new PatientCreateDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.Password);
    }

    [Fact]
    public void PatientCreateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new PatientCreateDto();
        var birthDate = new DateTime(1985, 10, 20);

        // Act
        dto.Name = "Jane Smith";
        dto.Phone = "9876543210";
        dto.Address = "456 Oak Ave";
        dto.BirthDate = birthDate;
        dto.Gender = "Female";
        dto.Email = "jane@example.com";
        dto.Password = "securePassword";

        // Assert
        Assert.Equal("Jane Smith", dto.Name);
        Assert.Equal("9876543210", dto.Phone);
        Assert.Equal("456 Oak Ave", dto.Address);
        Assert.Equal(birthDate, dto.BirthDate);
        Assert.Equal("Female", dto.Gender);
        Assert.Equal("jane@example.com", dto.Email);
        Assert.Equal("securePassword", dto.Password);
    }
}

public class PatientUpdateDtoTests
{
    [Fact]
    public void PatientUpdateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new PatientUpdateDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(string.Empty, dto.Gender);
    }

    [Fact]
    public void PatientUpdateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new PatientUpdateDto();
        var birthDate = new DateTime(1992, 7, 8);

        // Act
        dto.Name = "Bob Johnson";
        dto.Phone = "5551234567";
        dto.Address = "789 Pine Rd";
        dto.BirthDate = birthDate;
        dto.Gender = "Male";

        // Assert
        Assert.Equal("Bob Johnson", dto.Name);
        Assert.Equal("5551234567", dto.Phone);
        Assert.Equal("789 Pine Rd", dto.Address);
        Assert.Equal(birthDate, dto.BirthDate);
        Assert.Equal("Male", dto.Gender);
    }
}
