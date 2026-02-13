using Xunit;
using ClinicManagement.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.ClinicManagement.Application.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddApplication_ShouldReturnServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddApplication();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IServiceCollection>(result);
    }

    [Fact]
    public void AddApplication_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();

        // Assert
        Assert.NotEmpty(services);
    }

    [Fact]
    public void AddApplication_ShouldRegisterAutoMapper()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplication();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
        Assert.NotNull(mapper);
    }
}
