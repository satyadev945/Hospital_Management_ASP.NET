using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ClinicManagement.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR if used for CQRS pattern
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Register AutoMapper if used for object mapping
        // services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Register application services here
        // services.AddScoped<IAppointmentService, AppointmentService>();
        // services.AddScoped<IClientService, ClientService>();
        // services.AddScoped<IDoctorService, DoctorService>();
        // services.AddScoped<IPatientService, PatientService>();

        // Register validators if using FluentValidation
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
