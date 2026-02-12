using Xunit;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.DTOs.Tests;

public class OtherStaffDtoTests
{
    [Fact]
    public void OtherStaffDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new OtherStaffDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Designation);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(string.Empty, dto.Qualification);
    }

    [Fact]
    public void OtherStaffDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new OtherStaffDto();
        var birthDate = new DateTime(1985, 6, 10);

        // Act
        dto.StaffID = 1;
        dto.Name = "Jane Doe";
        dto.BirthDate = birthDate;
        dto.Phone = "5551234567";
        dto.Gender = "Female";
        dto.Designation = "Nurse";
        dto.Address = "123 Care St";
        dto.Salary = 60000m;
        dto.Qualification = "BSN";

        // Assert
        Assert.Equal(1, dto.StaffID);
        Assert.Equal("Jane Doe", dto.Name);
        Assert.Equal(birthDate, dto.BirthDate);
        Assert.Equal("5551234567", dto.Phone);
        Assert.Equal("Female", dto.Gender);
        Assert.Equal("Nurse", dto.Designation);
        Assert.Equal("123 Care St", dto.Address);
        Assert.Equal(60000m, dto.Salary);
        Assert.Equal("BSN", dto.Qualification);
    }

    [Fact]
    public void OtherStaffDto_Salary_CanBeDecimal()
    {
        // Arrange
        var dto = new OtherStaffDto();

        // Act
        dto.Salary = 55000.50m;

        // Assert
        Assert.Equal(55000.50m, dto.Salary);
    }
}

public class OtherStaffCreateDtoTests
{
    [Fact]
    public void OtherStaffCreateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new OtherStaffCreateDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Designation);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(string.Empty, dto.Qualification);
    }

    [Fact]
    public void OtherStaffCreateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new OtherStaffCreateDto();
        var birthDate = new DateTime(1990, 8, 15);

        // Act
        dto.Name = "Bob Smith";
        dto.BirthDate = birthDate;
        dto.Phone = "5559876543";
        dto.Gender = "Male";
        dto.Designation = "Technician";
        dto.Address = "456 Lab Rd";
        dto.Salary = 45000m;
        dto.Qualification = "Diploma";

        // Assert
        Assert.Equal("Bob Smith", dto.Name);
        Assert.Equal(birthDate, dto.BirthDate);
        Assert.Equal("5559876543", dto.Phone);
        Assert.Equal("Male", dto.Gender);
        Assert.Equal("Technician", dto.Designation);
        Assert.Equal("456 Lab Rd", dto.Address);
        Assert.Equal(45000m, dto.Salary);
        Assert.Equal("Diploma", dto.Qualification);
    }
}

public class OtherStaffUpdateDtoTests
{
    [Fact]
    public void OtherStaffUpdateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new OtherStaffUpdateDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Phone);
        Assert.Equal(string.Empty, dto.Designation);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(string.Empty, dto.Qualification);
    }

    [Fact]
    public void OtherStaffUpdateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new OtherStaffUpdateDto();

        // Act
        dto.Name = "Alice Brown";
        dto.Phone = "5554567890";
        dto.Designation = "Administrator";
        dto.Address = "789 Admin Blvd";
        dto.Salary = 70000m;
        dto.Qualification = "MBA";

        // Assert
        Assert.Equal("Alice Brown", dto.Name);
        Assert.Equal("5554567890", dto.Phone);
        Assert.Equal("Administrator", dto.Designation);
        Assert.Equal("789 Admin Blvd", dto.Address);
        Assert.Equal(70000m, dto.Salary);
        Assert.Equal("MBA", dto.Qualification);
    }
}
