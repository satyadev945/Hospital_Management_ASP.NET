using Xunit;
using ClinicManagement.Infrastructure.Data.Configurations;
using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tests.ClinicManagement.Infrastructure.Data.Configurations;

public class PatientConfigurationTests
{
    [Fact]
    public void PatientConfiguration_ShouldImplementIEntityTypeConfiguration()
    {
        // Arrange & Act
        var configuration = new PatientConfiguration();

        // Assert
        Assert.IsAssignableFrom<IEntityTypeConfiguration<Patient>>(configuration);
    }

    [Fact]
    public void PatientConfiguration_Configure_ShouldNotThrow()
    {
        // Arrange
        var configuration = new PatientConfiguration();
        var builder = CreateEntityTypeBuilder();

        // Act & Assert
        var exception = Record.Exception(() => configuration.Configure(builder));
        Assert.Null(exception);
    }

    private EntityTypeBuilder<Patient> CreateEntityTypeBuilder()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DbContext(options);
        var modelBuilder = new ModelBuilder();
        return new EntityTypeBuilder<Patient>(modelBuilder.Entity<Patient>().Metadata);
    }
}
