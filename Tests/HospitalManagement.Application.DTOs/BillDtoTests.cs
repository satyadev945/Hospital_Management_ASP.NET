using Xunit;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.DTOs.Tests;

public class BillDtoTests
{
    [Fact]
    public void BillDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new BillDto();

        // Assert
        Assert.Equal(string.Empty, dto.PatientName);
        Assert.False(dto.IsPaid);
    }

    [Fact]
    public void BillDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new BillDto();
        var billDate = new DateTime(2024, 12, 25);
        var paymentDate = new DateTime(2024, 12, 26);

        // Act
        dto.BillID = 1;
        dto.PatientID = 100;
        dto.PatientName = "John Doe";
        dto.AppointmentID = 200;
        dto.Amount = 5000m;
        dto.IsPaid = true;
        dto.BillDate = billDate;
        dto.PaymentDate = paymentDate;

        // Assert
        Assert.Equal(1, dto.BillID);
        Assert.Equal(100, dto.PatientID);
        Assert.Equal("John Doe", dto.PatientName);
        Assert.Equal(200, dto.AppointmentID);
        Assert.Equal(5000m, dto.Amount);
        Assert.True(dto.IsPaid);
        Assert.Equal(billDate, dto.BillDate);
        Assert.Equal(paymentDate, dto.PaymentDate);
    }

    [Fact]
    public void BillDto_PaymentDate_CanBeNull()
    {
        // Arrange & Act
        var dto = new BillDto();

        // Assert
        Assert.Null(dto.PaymentDate);
    }

    [Fact]
    public void BillDto_Amount_CanBeDecimal()
    {
        // Arrange
        var dto = new BillDto();

        // Act
        dto.Amount = 12345.67m;

        // Assert
        Assert.Equal(12345.67m, dto.Amount);
    }
}

public class BillCreateDtoTests
{
    [Fact]
    public void BillCreateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new BillCreateDto();

        // Assert
        Assert.Equal(0, dto.PatientID);
        Assert.Equal(0, dto.AppointmentID);
        Assert.Equal(0m, dto.Amount);
    }

    [Fact]
    public void BillCreateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new BillCreateDto();

        // Act
        dto.PatientID = 100;
        dto.AppointmentID = 200;
        dto.Amount = 7500m;

        // Assert
        Assert.Equal(100, dto.PatientID);
        Assert.Equal(200, dto.AppointmentID);
        Assert.Equal(7500m, dto.Amount);
    }

    [Fact]
    public void BillCreateDto_Amount_HandlesZero()
    {
        // Arrange
        var dto = new BillCreateDto();

        // Act
        dto.Amount = 0m;

        // Assert
        Assert.Equal(0m, dto.Amount);
    }

    [Fact]
    public void BillCreateDto_Amount_HandlesNegative()
    {
        // Arrange
        var dto = new BillCreateDto();

        // Act
        dto.Amount = -100m;

        // Assert
        Assert.Equal(-100m, dto.Amount);
    }
}

public class BillUpdateDtoTests
{
    [Fact]
    public void BillUpdateDto_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var dto = new BillUpdateDto();

        // Assert
        Assert.False(dto.IsPaid);
    }

    [Fact]
    public void BillUpdateDto_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var dto = new BillUpdateDto();
        var paymentDate = new DateTime(2024, 12, 30);

        // Act
        dto.IsPaid = true;
        dto.PaymentDate = paymentDate;

        // Assert
        Assert.True(dto.IsPaid);
        Assert.Equal(paymentDate, dto.PaymentDate);
    }

    [Fact]
    public void BillUpdateDto_PaymentDate_CanBeNull()
    {
        // Arrange & Act
        var dto = new BillUpdateDto();

        // Assert
        Assert.Null(dto.PaymentDate);
    }

    [Fact]
    public void BillUpdateDto_IsPaid_CanBeFalse()
    {
        // Arrange
        var dto = new BillUpdateDto();

        // Act
        dto.IsPaid = false;

        // Assert
        Assert.False(dto.IsPaid);
    }
}
