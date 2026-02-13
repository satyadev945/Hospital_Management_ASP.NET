using Xunit;
using ClinicManagement.Infrastructure.Data.Configurations;
using ClinicManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tests.ClinicManagement.Infrastructure.Data.Configurations;

public class UserConfigurationTests
{
    [Fact]
    public void UserConfiguration_ShouldImplementIEntityTypeConfiguration()
    {
        // Arrange & Act
        var configuration = new UserConfiguration();

        // Assert
        Assert.IsAssignableFrom<IEntityTypeConfiguration<User>>(configuration);
    }

    [Fact]
    public void UserConfiguration_Configure_ShouldNotThrow()
    {
        // Arrange
        var configuration = new UserConfiguration();
        var builder = CreateEntityTypeBuilder();

        // Act & Assert
        var exception = Record.Exception(() => configuration.Configure(builder));
        Assert.Null(exception);
    }

    private EntityTypeBuilder<User> CreateEntityTypeBuilder()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new DbContext(options);
        var modelBuilder = new ModelBuilder();
        return new EntityTypeBuilder<User>(modelBuilder.Entity<User>().Metadata);
    }
}
