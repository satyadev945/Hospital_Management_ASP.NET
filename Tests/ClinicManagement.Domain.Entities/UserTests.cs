using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using System;

namespace Tests.ClinicManagement.Domain.Entities;

public class UserTests
{
    [Fact]
    public void User_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.Name);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.Password);
        Assert.Equal(default(DateTime), user.BirthDate);
        Assert.Equal(string.Empty, user.PhoneNo);
        Assert.Equal(string.Empty, user.Gender);
        Assert.Equal(string.Empty, user.Address);
        Assert.Equal(default(UserType), user.UserType);
        Assert.Equal(default(DateTime), user.CreatedDate);
        Assert.Null(user.ModifiedDate);
        Assert.False(user.IsActive);
        Assert.Equal("System", user.CreatedBy);
        Assert.Null(user.ModifiedBy);
    }

    [Fact]
    public void User_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var user = new User();
        var now = DateTime.UtcNow;
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        user.Id = 1;
        user.Name = "John Doe";
        user.Email = "john@example.com";
        user.Password = "hashedpassword";
        user.BirthDate = birthDate;
        user.PhoneNo = "1234567890";
        user.Gender = "Male";
        user.Address = "123 Main St";
        user.UserType = UserType.Patient;
        user.CreatedDate = now;
        user.ModifiedDate = now;
        user.IsActive = true;
        user.CreatedBy = "Admin";
        user.ModifiedBy = "Admin";

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("John Doe", user.Name);
        Assert.Equal("john@example.com", user.Email);
        Assert.Equal("hashedpassword", user.Password);
        Assert.Equal(birthDate, user.BirthDate);
        Assert.Equal("1234567890", user.PhoneNo);
        Assert.Equal("Male", user.Gender);
        Assert.Equal("123 Main St", user.Address);
        Assert.Equal(UserType.Patient, user.UserType);
        Assert.Equal(now, user.CreatedDate);
        Assert.Equal(now, user.ModifiedDate);
        Assert.True(user.IsActive);
        Assert.Equal("Admin", user.CreatedBy);
        Assert.Equal("Admin", user.ModifiedBy);
    }

    [Theory]
    [InlineData(UserType.Patient)]
    [InlineData(UserType.Doctor)]
    [InlineData(UserType.Admin)]
    public void User_UserType_ShouldAcceptAllValidTypes(UserType userType)
    {
        // Arrange
        var user = new User();

        // Act
        user.UserType = userType;

        // Assert
        Assert.Equal(userType, user.UserType);
    }

    [Fact]
    public void User_IsActive_ShouldToggle()
    {
        // Arrange
        var user = new User { IsActive = false };

        // Act
        user.IsActive = true;

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_Email_ShouldAcceptValidFormat()
    {
        // Arrange
        var user = new User();

        // Act
        user.Email = "test@domain.com";

        // Assert
        Assert.Equal("test@domain.com", user.Email);
    }

    [Fact]
    public void User_ModifiedDate_CanBeNull()
    {
        // Arrange
        var user = new User { ModifiedDate = null };

        // Assert
        Assert.Null(user.ModifiedDate);
    }

    [Fact]
    public void User_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal("System", user.CreatedBy);
    }

    [Fact]
    public void User_ModifiedBy_CanBeNull()
    {
        // Arrange
        var user = new User { ModifiedBy = null };

        // Assert
        Assert.Null(user.ModifiedBy);
    }

    [Theory]
    [InlineData("Male")]
    [InlineData("Female")]
    [InlineData("Other")]
    public void User_Gender_ShouldAcceptValidValues(string gender)
    {
        // Arrange
        var user = new User();

        // Act
        user.Gender = gender;

        // Assert
        Assert.Equal(gender, user.Gender);
    }

    [Fact]
    public void User_BirthDate_ShouldAcceptValidDate()
    {
        // Arrange
        var user = new User();
        var birthDate = new DateTime(1995, 5, 15);

        // Act
        user.BirthDate = birthDate;

        // Assert
        Assert.Equal(birthDate, user.BirthDate);
    }
}
