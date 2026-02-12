using Xunit;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.DTOs.Tests;

public class DepartmentDtoTests
{
    [Fact]
    public void DepartmentDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new DepartmentDto();

        // Assert
        Assert.Equal(string.Empty, dto.DeptName);
    }

    [Fact]
    public void DepartmentDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new DepartmentDto();

        // Act
        dto.DeptNo = 1;
        dto.DeptName = "Cardiology";
        dto.Description = "Heart care department";

        // Assert
        Assert.Equal(1, dto.DeptNo);
        Assert.Equal("Cardiology", dto.DeptName);
        Assert.Equal("Heart care department", dto.Description);
    }

    [Fact]
    public void DepartmentDto_Description_CanBeNull()
    {
        // Arrange & Act
        var dto = new DepartmentDto();

        // Assert
        Assert.Null(dto.Description);
    }
}

public class DepartmentCreateDtoTests
{
    [Fact]
    public void DepartmentCreateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new DepartmentCreateDto();

        // Assert
        Assert.Equal(string.Empty, dto.DeptName);
    }

    [Fact]
    public void DepartmentCreateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new DepartmentCreateDto();

        // Act
        dto.DeptName = "Neurology";
        dto.Description = "Brain and nervous system care";

        // Assert
        Assert.Equal("Neurology", dto.DeptName);
        Assert.Equal("Brain and nervous system care", dto.Description);
    }

    [Fact]
    public void DepartmentCreateDto_Description_CanBeNull()
    {
        // Arrange & Act
        var dto = new DepartmentCreateDto();

        // Assert
        Assert.Null(dto.Description);
    }
}

public class DepartmentUpdateDtoTests
{
    [Fact]
    public void DepartmentUpdateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new DepartmentUpdateDto();

        // Assert
        Assert.Equal(string.Empty, dto.DeptName);
    }

    [Fact]
    public void DepartmentUpdateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new DepartmentUpdateDto();

        // Act
        dto.DeptName = "Orthopedics";
        dto.Description = "Bone and joint care";

        // Assert
        Assert.Equal("Orthopedics", dto.DeptName);
        Assert.Equal("Bone and joint care", dto.Description);
    }

    [Fact]
    public void DepartmentUpdateDto_Description_CanBeNull()
    {
        // Arrange & Act
        var dto = new DepartmentUpdateDto();

        // Assert
        Assert.Null(dto.Description);
    }
}
