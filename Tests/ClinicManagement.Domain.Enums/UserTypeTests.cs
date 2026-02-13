using Xunit;
using ClinicManagement.Domain.Enums;

namespace Tests.ClinicManagement.Domain.Enums;

public class UserTypeTests
{
    [Fact]
    public void UserType_PatientValue_ShouldBeOne()
    {
        // Arrange & Act
        var userType = UserType.Patient;

        // Assert
        Assert.Equal(1, (int)userType);
    }

    [Fact]
    public void UserType_DoctorValue_ShouldBeTwo()
    {
        // Arrange & Act
        var userType = UserType.Doctor;

        // Assert
        Assert.Equal(2, (int)userType);
    }

    [Fact]
    public void UserType_AdminValue_ShouldBeThree()
    {
        // Arrange & Act
        var userType = UserType.Admin;

        // Assert
        Assert.Equal(3, (int)userType);
    }

    [Fact]
    public void UserType_AllValues_ShouldBeDefined()
    {
        // Arrange & Act
        var values = Enum.GetValues<UserType>();

        // Assert
        Assert.Equal(3, values.Length);
        Assert.Contains(UserType.Patient, values);
        Assert.Contains(UserType.Doctor, values);
        Assert.Contains(UserType.Admin, values);
    }

    [Theory]
    [InlineData(1, UserType.Patient)]
    [InlineData(2, UserType.Doctor)]
    [InlineData(3, UserType.Admin)]
    public void UserType_IntToEnum_ShouldConvertCorrectly(int value, UserType expected)
    {
        // Arrange & Act
        var userType = (UserType)value;

        // Assert
        Assert.Equal(expected, userType);
    }

    [Fact]
    public void UserType_ToString_ShouldReturnEnumName()
    {
        // Arrange
        var userType = UserType.Doctor;

        // Act
        var result = userType.ToString();

        // Assert
        Assert.Equal("Doctor", result);
    }

    [Theory]
    [InlineData("Patient", UserType.Patient)]
    [InlineData("Doctor", UserType.Doctor)]
    [InlineData("Admin", UserType.Admin)]
    public void UserType_Parse_ShouldConvertStringToEnum(string value, UserType expected)
    {
        // Arrange & Act
        var userType = Enum.Parse<UserType>(value);

        // Assert
        Assert.Equal(expected, userType);
    }
}
