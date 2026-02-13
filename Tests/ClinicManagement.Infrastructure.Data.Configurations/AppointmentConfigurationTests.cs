using Xunit;
using ClinicManagement.Infrastructure.Data.Configurations;
using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tests.ClinicManagement.Infrastructure.Data.Configurations;

public class AppointmentConfigurationTests
{
    [Fact]
    public void AppointmentConfiguration_ShouldImplementIEntityTypeConfiguration()
    {
        // Arrange & Act
        var configuration = new AppointmentConfiguration();

        // Assert
        Assert.IsAssignableFrom<IEntityTypeConfiguration<Appointment>>(configuration);
    }

    [Fact]
    public void AppointmentConfiguration_Configure_ShouldNotThrow()
    {
        // Arrange
        var configuration = new AppointmentConfiguration();
        var builder = CreateEntityTypeBuilder();

        // Act & Assert
        var exception = Record.Exception(() => configuration.Configure(builder));
        Assert.Null(exception);
    }

    private EntityTypeBuilder<Appointment> CreateEntityTypeBuilder()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DbContext(options);
        var modelBuilder = new ModelBuilder();
        return new EntityTypeBuilder<Appointment>(modelBuilder.Entity<Appointment>().Metadata);
    }
}
