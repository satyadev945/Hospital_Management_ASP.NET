using ClinicManagement.Application.Interfaces;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext is registered in Program.cs, but we keep this method for consistency
        // and future infrastructure services

        // Register all repositories
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();

        // Register unit of work pattern if needed
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServicesWithDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Get connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // Register DbContext
        services.AddDbContext<ClinicManagementDbContext>(options =>
            options.UseSqlServer(connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(60);
                    sqlOptions.MigrationsAssembly(typeof(ClinicManagementDbContext).Assembly.FullName);
                }));

        // Register repositories
        return services.AddInfrastructureServices(configuration);
    }
}
