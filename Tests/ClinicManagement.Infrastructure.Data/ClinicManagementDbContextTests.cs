using Xunit;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Tests.ClinicManagement.Infrastructure.Data;

public class ClinicManagementDbContextTests
{
    [Fact]
    public void ClinicManagementDbContext_Constructor_ShouldInitialize()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ClinicManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new ClinicManagementDbContext(options);

        // Assert
        Assert.NotNull(context);
    }

    [Fact]
    public void ClinicManagementDbContext_Users_ShouldBeAccessible()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ClinicManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new ClinicManagementDbContext(options);

        // Assert
        Assert.NotNull(context.Users);
    }

    [Fact]
    public void ClinicManagementDbContext_Patients_ShouldBeAccessible()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ClinicManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new ClinicManagementDbContext(options);

        // Assert
        Assert.NotNull(context.Patients);
    }

    [Fact]
    public void ClinicManagementDbContext_Doctors_ShouldBeAccessible()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ClinicManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new ClinicManagementDbContext(options);

        // Assert
        Assert.NotNull(context.Doctors);
    }

    [Fact]
    public void ClinicManagementDbContext_Appointments_ShouldBeAccessible()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ClinicManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new ClinicManagementDbContext(options);

        // Assert
        Assert.NotNull(context.Appointments);
    }

    [Fact]
    public void ClinicManagementDbContext_Bills_ShouldBeAccessible()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ClinicManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new ClinicManagementDbContext(options);

        // Assert
        Assert.NotNull(context.Bills);
    }

    [Fact]
    public void ClinicManagementDbContext_InheritsFromDbContext()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ClinicManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Act
        using var context = new ClinicManagementDbContext(options);

        // Assert
        Assert.IsAssignableFrom<DbContext>(context);
    }
}
