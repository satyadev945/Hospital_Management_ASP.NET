using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagement.Infrastructure.Data;

namespace HospitalManagement.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring Infrastructure layer services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Infrastructure layer services to the dependency injection container
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <returns>The service collection for method chaining</returns>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register DbContext with SQL Server
            services.AddDbContext<HospitalDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        sqlOptions.CommandTimeout(30);
                    });

                // Enable sensitive data logging in development
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environment == "Development")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            // Register repositories (add as they are created)
            // Example: services.AddScoped<IPatientRepository, PatientRepository>();
            // Example: services.AddScoped<IDoctorRepository, DoctorRepository>();
            // Example: services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            // Example: services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            // Example: services.AddScoped<IOtherStaffRepository, OtherStaffRepository>();
            // Example: services.AddScoped<IBillRepository, BillRepository>();
            // Example: services.AddScoped<ITreatmentHistoryRepository, TreatmentHistoryRepository>();
            // Example: services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            // Register Unit of Work pattern (if implemented)
            // Example: services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Adds Infrastructure layer services with a custom connection string
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="connectionString">The database connection string</param>
        /// <returns>The service collection for method chaining</returns>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            string connectionString)
        {
            // Register DbContext with SQL Server using connection string
            services.AddDbContext<HospitalDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        sqlOptions.CommandTimeout(30);
                    });

                // Enable sensitive data logging in development
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environment == "Development")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            // Register repositories (add as they are created)
            // Example: services.AddScoped<IPatientRepository, PatientRepository>();
            // Example: services.AddScoped<IDoctorRepository, DoctorRepository>();
            // Example: services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            // Example: services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            // Example: services.AddScoped<IOtherStaffRepository, OtherStaffRepository>();
            // Example: services.AddScoped<IBillRepository, BillRepository>();
            // Example: services.AddScoped<ITreatmentHistoryRepository, TreatmentHistoryRepository>();
            // Example: services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            // Register Unit of Work pattern (if implemented)
            // Example: services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Configures additional database options for advanced scenarios
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The application configuration</param>
        /// <param name="configureOptions">Additional options configuration</param>
        /// <returns>The service collection for method chaining</returns>
        public static IServiceCollection AddInfrastructureServicesWithOptions(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<DbContextOptionsBuilder> configureOptions)
        {
            // Register DbContext with custom options
            services.AddDbContext<HospitalDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        sqlOptions.CommandTimeout(30);
                    });

                // Apply custom options
                configureOptions?.Invoke(options);
            });

            // Register repositories (add as they are created)
            // Example: services.AddScoped<IPatientRepository, PatientRepository>();
            // Example: services.AddScoped<IDoctorRepository, DoctorRepository>();
            // Example: services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            // Example: services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            // Example: services.AddScoped<IOtherStaffRepository, OtherStaffRepository>();
            // Example: services.AddScoped<IBillRepository, BillRepository>();
            // Example: services.AddScoped<ITreatmentHistoryRepository, TreatmentHistoryRepository>();
            // Example: services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            // Register Unit of Work pattern (if implemented)
            // Example: services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
