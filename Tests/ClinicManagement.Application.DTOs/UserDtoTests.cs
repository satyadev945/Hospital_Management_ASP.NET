using Xunit;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Enums;
using System;

namespace Tests.ClinicManagement.Application.DTOs;

public class UserDtoTests
{
    [Fact]
    public void UserDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new UserDto();

        // Assert
        Assert.Equal(0, dto.Id);
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(default(DateTime), dto.BirthDate);
        Assert.Equal(string.Empty, dto.PhoneNo);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(default(UserType), dto.UserType);
        Assert.False(dto.IsActive);
        Assert.Equal(default(DateTime), dto.CreatedDate);
    }

    [Fact]
    public void UserDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var dto = new UserDto();
        var now = DateTime.UtcNow;
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        dto.Id = 1;
        dto.Name = "John Doe";
        dto.Email = "john@example.com";
        dto.BirthDate = birthDate;
        dto.PhoneNo = "1234567890";
        dto.Gender = "Male";
        dto.Address = "123 Main St";
        dto.UserType = UserType.Patient;
        dto.IsActive = true;
        dto.CreatedDate = now;

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("John Doe", dto.Name);
        Assert.Equal("john@example.com", dto.Email);
        Assert.Equal(birthDate, dto.BirthDate);
        Assert.Equal("1234567890", dto.PhoneNo);
        Assert.Equal("Male", dto.Gender);
        Assert.Equal("123 Main St", dto.Address);
        Assert.Equal(UserType.Patient, dto.UserType);
        Assert.True(dto.IsActive);
        Assert.Equal(now, dto.CreatedDate);
    }

    [Fact]
    public void UserCreateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new UserCreateDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.Password);
        Assert.Equal(default(DateTime), dto.BirthDate);
        Assert.Equal(string.Empty, dto.PhoneNo);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Address);
        Assert.Equal(default(UserType), dto.UserType);
    }

    [Fact]
    public void UserCreateDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var dto = new UserCreateDto();
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        dto.Name = "Jane Doe";
        dto.Email = "jane@example.com";
        dto.Password = "securepassword";
        dto.BirthDate = birthDate;
        dto.PhoneNo = "9876543210";
        dto.Gender = "Female";
        dto.Address = "456 Elm St";
        dto.UserType = UserType.Doctor;

        // Assert
        Assert.Equal("Jane Doe", dto.Name);
        Assert.Equal("jane@example.com", dto.Email);
        Assert.Equal("securepassword", dto.Password);
        Assert.Equal(birthDate, dto.BirthDate);
        Assert.Equal("9876543210", dto.PhoneNo);
        Assert.Equal("Female", dto.Gender);
        Assert.Equal("456 Elm St", dto.Address);
        Assert.Equal(UserType.Doctor, dto.UserType);
    }

    [Fact]
    public void UserUpdateDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new UserUpdateDto();

        // Assert
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(default(DateTime), dto.BirthDate);
        Assert.Equal(string.Empty, dto.PhoneNo);
        Assert.Equal(string.Empty, dto.Gender);
        Assert.Equal(string.Empty, dto.Address);
    }

    [Fact]
    public void LoginDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new LoginDto();

        // Assert
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.Password);
    }

    [Fact]
    public void LoginDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var dto = new LoginDto();

        // Act
        dto.Email = "user@example.com";
        dto.Password = "mypassword";

        // Assert
        Assert.Equal("user@example.com", dto.Email);
        Assert.Equal("mypassword", dto.Password);
    }

    [Fact]
    public void LoginResponseDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var dto = new LoginResponseDto();

        // Assert
        Assert.False(dto.Success);
        Assert.Equal(0, dto.UserId);
        Assert.Equal(default(UserType), dto.UserType);
        Assert.Equal(string.Empty, dto.Message);
    }

    [Fact]
    public void LoginResponseDto_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var dto = new LoginResponseDto();

        // Act
        dto.Success = true;
        dto.UserId = 100;
        dto.UserType = UserType.Admin;
        dto.Message = "Login successful";

        // Assert
        Assert.True(dto.Success);
        Assert.Equal(100, dto.UserId);
        Assert.Equal(UserType.Admin, dto.UserType);
        Assert.Equal("Login successful", dto.Message);
    }
}
