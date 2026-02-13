using ClinicManagement.Application.Services;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagement.Application.Extensions;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application services to the DI container
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
