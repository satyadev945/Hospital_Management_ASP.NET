using HospitalManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Application.Extensions;

/// <summary>
/// Extension methods for registering application services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());

        services.AddScoped<Interfaces.IPatientService, PatientService>();
        services.AddScoped<Interfaces.IDoctorService, DoctorService>();
        services.AddScoped<Interfaces.IAppointmentService, AppointmentService>();
        services.AddScoped<Interfaces.IBillService, BillService>();
        services.AddScoped<Interfaces.IDepartmentService, DepartmentService>();
        services.AddScoped<Interfaces.IOtherStaffService, OtherStaffService>();

        return services;
    }
}
