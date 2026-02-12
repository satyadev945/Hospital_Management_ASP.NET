using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class BillTests
{
    [Fact]
    public void Bill_Constructor_InitializesDefaultValues()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.False(bill.IsPaid);
        Assert.Equal("System", bill.CreatedBy);
    }

    [Fact]
    public void Bill_SetProperties_StoresValuesCorrectly()
    {
        // Arrange
        var bill = new Bill();
        var billDate = new DateTime(2024, 12, 25);
        var paymentDate = new DateTime(2024, 12, 26);

        // Act
        bill.BillID = 1;
        bill.PatientID = 100;
        bill.AppointmentID = 200;
        bill.Amount = 5000m;
        bill.IsPaid = true;
        bill.BillDate = billDate;
        bill.PaymentDate = paymentDate;

        // Assert
        Assert.Equal(1, bill.BillID);
        Assert.Equal(100, bill.PatientID);
        Assert.Equal(200, bill.AppointmentID);
        Assert.Equal(5000m, bill.Amount);
        Assert.True(bill.IsPaid);
        Assert.Equal(billDate, bill.BillDate);
        Assert.Equal(paymentDate, bill.PaymentDate);
    }

    [Fact]
    public void Bill_IsPaid_DefaultsToFalse()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.False(bill.IsPaid);
    }

    [Fact]
    public void Bill_Amount_CanBeDecimal()
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Amount = 12345.67m;

        // Assert
        Assert.Equal(12345.67m, bill.Amount);
    }

    [Fact]
    public void Bill_Amount_DefaultsToZero()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Equal(0m, bill.Amount);
    }

    [Fact]
    public void Bill_PaymentDate_CanBeNull()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Null(bill.PaymentDate);
    }

    [Fact]
    public void Bill_PaymentDate_CanBeSet()
    {
        // Arrange
        var bill = new Bill();
        var paymentDate = DateTime.UtcNow;

        // Act
        bill.PaymentDate = paymentDate;

        // Assert
        Assert.Equal(paymentDate, bill.PaymentDate);
    }

    [Fact]
    public void Bill_Patient_CanBeNull()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Null(bill.Patient);
    }

    [Fact]
    public void Bill_Appointment_CanBeNull()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Null(bill.Appointment);
    }

    [Fact]
    public void Bill_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Null(bill.ModifiedDate);
    }

    [Fact]
    public void Bill_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Null(bill.ModifiedBy);
    }

    [Fact]
    public void Bill_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Equal("System", bill.CreatedBy);
    }
}
