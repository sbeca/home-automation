using Microsoft.EntityFrameworkCore;

namespace AirGradientDataServer.Models
{
    public class MeasurementContext : DbContext
    {
        public MeasurementContext(DbContextOptions<MeasurementContext> options)
            : base(options)
        {
        }

        public DbSet<Measurement> Measurements { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Auto-set the MeasurementTime value to the server's now value,
            // instead of relying on the AirGradient device for times
            modelBuilder.Entity<Measurement>()
                .Property(b => b.MeasurementTime)
                .HasDefaultValueSql("datetime('now')");
        }
    }
}
