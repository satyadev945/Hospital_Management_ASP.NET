using Xunit;
using ClinicManagement.Infrastructure.Data.Configurations;
using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tests.ClinicManagement.Infrastructure.Data.Configurations;

public class DoctorConfigurationTests
{
    [Fact]
    public void DoctorConfiguration_ShouldImplementIEntityTypeConfiguration()
    {
        // Arrange & Act
        var configuration = new DoctorConfiguration();

        // Assert
        Assert.IsAssignableFrom<IEntityTypeConfiguration<Doctor>>(configuration);
    }

    [Fact]
    public void DoctorConfiguration_Configure_ShouldNotThrow()
    {
        // Arrange
        var configuration = new DoctorConfiguration();
        var builder = CreateEntityTypeBuilder();

        // Act & Assert
        var exception = Record.Exception(() => configuration.Configure(builder));
        Assert.Null(exception);
    }

    private EntityTypeBuilder<Doctor> CreateEntityTypeBuilder()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DbContext(options);
        var modelBuilder = new ModelBuilder();
        return new EntityTypeBuilder<Doctor>(modelBuilder.Entity<Doctor>().Metadata);
    }
}
