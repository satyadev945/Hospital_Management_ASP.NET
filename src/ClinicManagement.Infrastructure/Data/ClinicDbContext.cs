using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Data;

/// <summary>
/// Database context for the Clinic Management System.
/// Provides access to all entity DbSets and configures entity relationships.
/// </summary>
public class ClinicDbContext : DbContext
{
    private readonly ILogger<ClinicDbContext>? _logger;

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
    {
    }

    public ClinicDbContext(
        DbContextOptions<ClinicDbContext> options,
        ILogger<ClinicDbContext> logger) : base(options)
    {
        _logger = logger;
    }

    // DbSets for all entities
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<Staff> Staff => Set<Staff>();

    /// <summary>
    /// Configures entity mappings and relationships using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        try
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
            modelBuilder.ApplyConfiguration(new BillConfiguration());
            modelBuilder.ApplyConfiguration(new StaffConfiguration());

            _logger?.LogDebug("Entity configurations applied successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error occurred while configuring entity models");
            throw;
        }
    }

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            _logger?.LogDebug("Successfully saved {Count} changes to database", result);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger?.LogError(ex, "Concurrency conflict occurred while saving changes");
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger?.LogError(ex, "Database update error occurred while saving changes");
            throw;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unexpected error occurred while saving changes");
            throw;
        }
    }

    /// <summary>
    /// Configures the database connection and logging.
    /// </summary>
    /// <param name="optionsBuilder">Options builder for configuring the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (_logger != null)
        {
            optionsBuilder.LogTo(
                message => _logger.LogDebug("{Message}", message),
                LogLevel.Information);
        }
    }
}
