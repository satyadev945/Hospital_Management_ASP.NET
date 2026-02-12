using Microsoft.EntityFrameworkCore;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data.Configurations;

namespace HospitalManagement.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework Core database context for Hospital Management System
    /// </summary>
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options)
            : base(options)
        {
        }

        // DbSet Properties
        public DbSet<Login> Logins { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<OtherStaff> OtherStaff { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<TreatmentHistory> TreatmentHistories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorConfiguration());
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new OtherStaffConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
            modelBuilder.ApplyConfiguration(new BillConfiguration());
            modelBuilder.ApplyConfiguration(new TreatmentHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
        }

        /// <summary>
        /// Override SaveChanges to automatically set audit fields
        /// </summary>
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync to automatically set audit fields
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates audit fields (CreatedDate, ModifiedDate) for tracked entities
        /// </summary>
        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Property("CreatedDate").CurrentValue == null ||
                        (DateTime)entry.Property("CreatedDate").CurrentValue == default)
                    {
                        entry.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                    }
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("ModifiedDate").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
