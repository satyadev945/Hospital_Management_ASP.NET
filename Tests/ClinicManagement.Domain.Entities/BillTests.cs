using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace Tests.ClinicManagement.Domain.Entities;

public class BillTests
{
    [Fact]
    public void Bill_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Equal(0, bill.Id);
        Assert.Equal(0, bill.AppointmentId);
        Assert.Null(bill.Appointment);
        Assert.Equal(0m, bill.Amount);
        Assert.Equal(string.Empty, bill.Description);
        Assert.False(bill.IsPaid);
        Assert.Equal(default(DateTime), bill.CreatedDate);
        Assert.Null(bill.ModifiedDate);
        Assert.False(bill.IsActive);
        Assert.Equal("System", bill.CreatedBy);
        Assert.Null(bill.ModifiedBy);
    }

    [Fact]
    public void Bill_SetProperties_ShouldUpdateValues()
    {
        // Arrange
        var bill = new Bill();
        var now = DateTime.UtcNow;

        // Act
        bill.Id = 1;
        bill.AppointmentId = 100;
        bill.Amount = 150.75m;
        bill.Description = "Consultation fee";
        bill.IsPaid = true;
        bill.CreatedDate = now;
        bill.ModifiedDate = now;
        bill.IsActive = true;
        bill.CreatedBy = "Admin";
        bill.ModifiedBy = "Billing";

        // Assert
        Assert.Equal(1, bill.Id);
        Assert.Equal(100, bill.AppointmentId);
        Assert.Equal(150.75m, bill.Amount);
        Assert.Equal("Consultation fee", bill.Description);
        Assert.True(bill.IsPaid);
        Assert.Equal(now, bill.CreatedDate);
        Assert.Equal(now, bill.ModifiedDate);
        Assert.True(bill.IsActive);
        Assert.Equal("Admin", bill.CreatedBy);
        Assert.Equal("Billing", bill.ModifiedBy);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50.00)]
    [InlineData(100.50)]
    [InlineData(1000.99)]
    public void Bill_Amount_ShouldAcceptValidValues(decimal amount)
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.Amount = amount;

        // Assert
        Assert.Equal(amount, bill.Amount);
    }

    [Fact]
    public void Bill_IsPaid_ShouldToggle()
    {
        // Arrange
        var bill = new Bill { IsPaid = false };

        // Act
        bill.IsPaid = true;

        // Assert
        Assert.True(bill.IsPaid);
    }

    [Fact]
    public void Bill_AppointmentNavigationProperty_ShouldBeSettable()
    {
        // Arrange
        var bill = new Bill();
        var appointment = new Appointment { Id = 100 };

        // Act
        bill.Appointment = appointment;

        // Assert
        Assert.NotNull(bill.Appointment);
        Assert.Equal(100, bill.Appointment.Id);
    }

    [Fact]
    public void Bill_IsActive_ShouldToggle()
    {
        // Arrange
        var bill = new Bill { IsActive = false };

        // Act
        bill.IsActive = true;

        // Assert
        Assert.True(bill.IsActive);
    }

    [Fact]
    public void Bill_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var bill = new Bill();

        // Assert
        Assert.Equal("System", bill.CreatedBy);
    }

    [Fact]
    public void Bill_ModifiedDate_CanBeNull()
    {
        // Arrange
        var bill = new Bill { ModifiedDate = null };

        // Assert
        Assert.Null(bill.ModifiedDate);
    }

    [Fact]
    public void Bill_Description_ShouldAcceptLongStrings()
    {
        // Arrange
        var bill = new Bill();
        var longDescription = new string('A', 500);

        // Act
        bill.Description = longDescription;

        // Assert
        Assert.Equal(longDescription, bill.Description);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Bill_IsPaid_ShouldAcceptBothValues(bool isPaid)
    {
        // Arrange
        var bill = new Bill();

        // Act
        bill.IsPaid = isPaid;

        // Assert
        Assert.Equal(isPaid, bill.IsPaid);
    }
}
