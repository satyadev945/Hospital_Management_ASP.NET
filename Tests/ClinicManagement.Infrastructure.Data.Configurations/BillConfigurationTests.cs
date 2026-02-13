using Xunit;
using ClinicManagement.Infrastructure.Data.Configurations;
using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tests.ClinicManagement.Infrastructure.Data.Configurations;

public class BillConfigurationTests
{
    [Fact]
    public void BillConfiguration_ShouldImplementIEntityTypeConfiguration()
    {
        // Arrange & Act
        var configuration = new BillConfiguration();

        // Assert
        Assert.IsAssignableFrom<IEntityTypeConfiguration<Bill>>(configuration);
    }

    [Fact]
    public void BillConfiguration_Configure_ShouldNotThrow()
    {
        // Arrange
        var configuration = new BillConfiguration();
        var builder = CreateEntityTypeBuilder();

        // Act & Assert
        var exception = Record.Exception(() => configuration.Configure(builder));
        Assert.Null(exception);
    }

    private EntityTypeBuilder<Bill> CreateEntityTypeBuilder()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DbContext(options);
        var modelBuilder = new ModelBuilder();
        return new EntityTypeBuilder<Bill>(modelBuilder.Entity<Bill>().Metadata);
    }
}
