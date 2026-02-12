using HospitalManagement.Application;
using HospitalManagement.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/hospitalmanagement-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddRazorPages();

// Add Application and Infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Register service implementations
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Services.IPatientService, HospitalManagement.Application.Services.PatientService>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Services.IDoctorService, HospitalManagement.Application.Services.DoctorService>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Services.IDepartmentService, HospitalManagement.Application.Services.DepartmentService>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Services.IAppointmentService, HospitalManagement.Application.Services.AppointmentService>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Services.IStaffService, HospitalManagement.Application.Services.StaffService>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Services.IAuthService, HospitalManagement.Application.Services.AuthService>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Services.IDashboardService, HospitalManagement.Application.Services.DashboardService>();

// Register repository implementations
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Repositories.IPatientRepository, HospitalManagement.Infrastructure.Repositories.PatientRepository>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Repositories.IDoctorRepository, HospitalManagement.Infrastructure.Repositories.DoctorRepository>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Repositories.IDepartmentRepository, HospitalManagement.Infrastructure.Repositories.DepartmentRepository>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Repositories.IAppointmentRepository, HospitalManagement.Infrastructure.Repositories.AppointmentRepository>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Repositories.IOtherStaffRepository, HospitalManagement.Infrastructure.Repositories.OtherStaffRepository>();
builder.Services.AddScoped<HospitalManagement.Domain.Interfaces.Repositories.ILoginRepository, HospitalManagement.Infrastructure.Repositories.LoginRepository>();

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("DoctorOnly", policy => policy.RequireRole("Doctor"));
    options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
    options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff"));
});

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseSerilogRequestLogging();

app.MapRazorPages();

// Default route
app.MapGet("/", async context =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        if (context.User.IsInRole("Admin"))
        {
            context.Response.Redirect("/Admin/Index");
        }
        else if (context.User.IsInRole("Doctor"))
        {
            context.Response.Redirect("/Doctor/Index");
        }
        else if (context.User.IsInRole("Patient"))
        {
            context.Response.Redirect("/Patient/Index");
        }
        else
        {
            context.Response.Redirect("/Login");
        }
    }
    else
    {
        context.Response.Redirect("/Login");
    }
});

try
{
    Log.Information("Starting Hospital Management System");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
