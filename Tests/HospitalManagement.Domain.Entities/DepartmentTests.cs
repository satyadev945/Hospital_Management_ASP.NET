using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class DepartmentTests
{
    [Fact]
    public void Department_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var department = new Department();

        // Assert
        Assert.Equal(string.Empty, department.DeptName);
        Assert.True(department.IsActive);
        Assert.Equal("System", department.CreatedBy);
        Assert.NotNull(department.Doctors);
    }

    [Fact]
    public void Department_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var department = new Department();

        // Act
        department.DeptNo = 1;
        department.DeptName = "Cardiology";
        department.Description = "Heart and cardiovascular care";
        department.IsActive = false;

        // Assert
        Assert.Equal(1, department.DeptNo);
        Assert.Equal("Cardiology", department.DeptName);
        Assert.Equal("Heart and cardiovascular care", department.Description);
        Assert.False(department.IsActive);
    }

    [Fact]
    public void Department_IsActive_DefaultsToTrue()
    {
        // Arrange & Act
        var department = new Department();

        // Assert
        Assert.True(department.IsActive);
    }

    [Fact]
    public void Department_Description_CanBeNull()
    {
        // Arrange & Act
        var department = new Department();

        // Assert
        Assert.Null(department.Description);
    }

    [Fact]
    public void Department_Description_CanBeSet()
    {
        // Arrange
        var department = new Department();

        // Act
        department.Description = "Test Description";

        // Assert
        Assert.Equal("Test Description", department.Description);
    }

    [Fact]
    public void Department_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var department = new Department();

        // Assert
        Assert.Null(department.ModifiedDate);
    }

    [Fact]
    public void Department_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var department = new Department();

        // Assert
        Assert.Null(department.ModifiedBy);
    }

    [Fact]
    public void Department_Doctors_InitializesAsEmptyList()
    {
        // Arrange & Act
        var department = new Department();

        // Assert
        Assert.NotNull(department.Doctors);
        Assert.Empty(department.Doctors);
    }

    [Fact]
    public void Department_Doctors_CanAddItems()
    {
        // Arrange
        var department = new Department();
        var doctor = new Doctor { DoctorID = 1, Name = "Dr. Smith" };

        // Act
        department.Doctors.Add(doctor);

        // Assert
        Assert.Single(department.Doctors);
        Assert.Contains(doctor, department.Doctors);
    }

    [Fact]
    public void Department_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var department = new Department();

        // Assert
        Assert.Equal("System", department.CreatedBy);
    }
}
