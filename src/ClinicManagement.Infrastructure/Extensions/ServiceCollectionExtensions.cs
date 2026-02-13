using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagement.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the DI container
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = GetConnectionString(configuration);

        services.AddDbContext<ClinicManagementDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IBillRepository, BillRepository>();

        return services;
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

        if (!string.IsNullOrEmpty(dbServer) && !string.IsNullOrEmpty(dbName) &&
            !string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
        {
            return $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";
        }

        return configuration.GetConnectionString("DefaultConnection")
               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
}
