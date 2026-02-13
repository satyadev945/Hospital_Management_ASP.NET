using Xunit;
using ClinicManagement.Domain.Enums;

namespace Tests.ClinicManagement.Domain.Enums;

public class AppointmentStatusTests
{
    [Fact]
    public void AppointmentStatus_PendingValue_ShouldBeZero()
    {
        // Arrange & Act
        var status = AppointmentStatus.Pending;

        // Assert
        Assert.Equal(0, (int)status);
    }

    [Fact]
    public void AppointmentStatus_ApprovedValue_ShouldBeOne()
    {
        // Arrange & Act
        var status = AppointmentStatus.Approved;

        // Assert
        Assert.Equal(1, (int)status);
    }

    [Fact]
    public void AppointmentStatus_CompletedValue_ShouldBeTwo()
    {
        // Arrange & Act
        var status = AppointmentStatus.Completed;

        // Assert
        Assert.Equal(2, (int)status);
    }

    [Fact]
    public void AppointmentStatus_CancelledValue_ShouldBeThree()
    {
        // Arrange & Act
        var status = AppointmentStatus.Cancelled;

        // Assert
        Assert.Equal(3, (int)status);
    }

    [Fact]
    public void AppointmentStatus_AllValues_ShouldBeDefined()
    {
        // Arrange & Act
        var values = Enum.GetValues<AppointmentStatus>();

        // Assert
        Assert.Equal(4, values.Length);
        Assert.Contains(AppointmentStatus.Pending, values);
        Assert.Contains(AppointmentStatus.Approved, values);
        Assert.Contains(AppointmentStatus.Completed, values);
        Assert.Contains(AppointmentStatus.Cancelled, values);
    }

    [Theory]
    [InlineData(0, AppointmentStatus.Pending)]
    [InlineData(1, AppointmentStatus.Approved)]
    [InlineData(2, AppointmentStatus.Completed)]
    [InlineData(3, AppointmentStatus.Cancelled)]
    public void AppointmentStatus_IntToEnum_ShouldConvertCorrectly(int value, AppointmentStatus expected)
    {
        // Arrange & Act
        var status = (AppointmentStatus)value;

        // Assert
        Assert.Equal(expected, status);
    }

    [Fact]
    public void AppointmentStatus_ToString_ShouldReturnEnumName()
    {
        // Arrange
        var status = AppointmentStatus.Pending;

        // Act
        var result = status.ToString();

        // Assert
        Assert.Equal("Pending", result);
    }
}
