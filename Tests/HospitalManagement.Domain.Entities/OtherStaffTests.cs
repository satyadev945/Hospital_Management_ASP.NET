using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class OtherStaffTests
{
    [Fact]
    public void OtherStaff_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var staff = new OtherStaff();

        // Assert
        Assert.Equal(string.Empty, staff.Name);
        Assert.Equal(string.Empty, staff.Phone);
        Assert.Equal(string.Empty, staff.Gender);
        Assert.Equal(string.Empty, staff.Designation);
        Assert.Equal(string.Empty, staff.Address);
        Assert.Equal(string.Empty, staff.Qualification);
        Assert.True(staff.Status);
        Assert.Equal("System", staff.CreatedBy);
    }

    [Fact]
    public void OtherStaff_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var staff = new OtherStaff();
        var birthDate = new DateTime(1985, 3, 20);

        // Act
        staff.StaffID = 1;
        staff.Name = "Jane Smith";
        staff.BirthDate = birthDate;
        staff.Phone = "5551234567";
        staff.Gender = "Female";
        staff.Designation = "Nurse";
        staff.Address = "789 Care St";
        staff.Salary = 50000m;
        staff.Qualification = "BSN";
        staff.Status = true;

        // Assert
        Assert.Equal(1, staff.StaffID);
        Assert.Equal("Jane Smith", staff.Name);
        Assert.Equal(birthDate, staff.BirthDate);
        Assert.Equal("5551234567", staff.Phone);
        Assert.Equal("Female", staff.Gender);
        Assert.Equal("Nurse", staff.Designation);
        Assert.Equal("789 Care St", staff.Address);
        Assert.Equal(50000m, staff.Salary);
        Assert.Equal("BSN", staff.Qualification);
        Assert.True(staff.Status);
    }

    [Fact]
    public void OtherStaff_Status_DefaultsToTrue()
    {
        // Arrange & Act
        var staff = new OtherStaff();

        // Assert
        Assert.True(staff.Status);
    }

    [Fact]
    public void OtherStaff_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var staff = new OtherStaff();

        // Assert
        Assert.Null(staff.ModifiedDate);
    }

    [Fact]
    public void OtherStaff_ModifiedDate_CanBeSet()
    {
        // Arrange
        var staff = new OtherStaff();
        var modifiedDate = DateTime.UtcNow;

        // Act
        staff.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, staff.ModifiedDate);
    }

    [Fact]
    public void OtherStaff_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var staff = new OtherStaff();

        // Assert
        Assert.Null(staff.ModifiedBy);
    }

    [Fact]
    public void OtherStaff_Salary_CanBeDecimal()
    {
        // Arrange
        var staff = new OtherStaff();

        // Act
        staff.Salary = 45000.50m;

        // Assert
        Assert.Equal(45000.50m, staff.Salary);
    }

    [Fact]
    public void OtherStaff_Salary_DefaultsToZero()
    {
        // Arrange & Act
        var staff = new OtherStaff();

        // Assert
        Assert.Equal(0m, staff.Salary);
    }
}
