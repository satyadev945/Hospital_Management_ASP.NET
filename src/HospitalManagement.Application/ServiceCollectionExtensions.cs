using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HospitalManagement.Application
{
    /// <summary>
    /// Extension methods for configuring Application layer services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Application layer services to the dependency injection container
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for method chaining</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register AutoMapper with all profiles in the Application assembly
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register FluentValidation validators
            // Note: Add validators as they are created
            // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register application services (add as they are created)
            // Example: services.AddScoped<IPatientService, PatientService>();
            // Example: services.AddScoped<IDoctorService, DoctorService>();
            // Example: services.AddScoped<IAppointmentService, AppointmentService>();
            // Example: services.AddScoped<IDepartmentService, DepartmentService>();

            return services;
        }
    }
}
