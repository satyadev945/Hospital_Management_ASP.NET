using Xunit;
using ClinicManagement.Domain.Exceptions;
using System;

namespace Tests.ClinicManagement.Domain.Exceptions;

public class ClinicManagementExceptionTests
{
    [Fact]
    public void ClinicManagementException_DefaultConstructor_ShouldCreate()
    {
        // Arrange & Act
        var exception = new ClinicManagementException();

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<ClinicManagementException>(exception);
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void ClinicManagementException_MessageConstructor_ShouldSetMessage()
    {
        // Arrange
        var message = "Test error message";

        // Act
        var exception = new ClinicManagementException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ClinicManagementException_MessageAndInnerExceptionConstructor_ShouldSetBoth()
    {
        // Arrange
        var message = "Test error message";
        var innerException = new InvalidOperationException("Inner error");

        // Act
        var exception = new ClinicManagementException(message, innerException);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }

    [Fact]
    public void EntityNotFoundException_Constructor_ShouldFormatMessage()
    {
        // Arrange
        var entityName = "User";
        var id = 123;

        // Act
        var exception = new EntityNotFoundException(entityName, id);

        // Assert
        Assert.Equal("User with ID 123 not found", exception.Message);
        Assert.IsType<EntityNotFoundException>(exception);
        Assert.IsAssignableFrom<ClinicManagementException>(exception);
    }

    [Theory]
    [InlineData("Patient", 1, "Patient with ID 1 not found")]
    [InlineData("Doctor", 99, "Doctor with ID 99 not found")]
    [InlineData("Appointment", 0, "Appointment with ID 0 not found")]
    public void EntityNotFoundException_Constructor_ShouldFormatMessageCorrectly(string entityName, int id, string expected)
    {
        // Arrange & Act
        var exception = new EntityNotFoundException(entityName, id);

        // Assert
        Assert.Equal(expected, exception.Message);
    }

    [Fact]
    public void ValidationException_Constructor_ShouldSetMessage()
    {
        // Arrange
        var message = "Validation failed";

        // Act
        var exception = new ValidationException(message);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.IsType<ValidationException>(exception);
        Assert.IsAssignableFrom<ClinicManagementException>(exception);
    }

    [Fact]
    public void ValidationException_Throw_ShouldBeCatchableAsClinicManagementException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ClinicManagementException>(() => throw new ValidationException("Test"));
        Assert.IsType<ValidationException>(exception);
    }

    [Fact]
    public void EntityNotFoundException_Throw_ShouldBeCatchableAsClinicManagementException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ClinicManagementException>(() => throw new EntityNotFoundException("User", 1));
        Assert.IsType<EntityNotFoundException>(exception);
    }

    [Fact]
    public void ValidationException_Message_ShouldBeAccessible()
    {
        // Arrange
        var message = "Email is required";
        var exception = new ValidationException(message);

        // Act & Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void ClinicManagementException_InheritsFromException()
    {
        // Arrange & Act
        var exception = new ClinicManagementException();

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }
}
