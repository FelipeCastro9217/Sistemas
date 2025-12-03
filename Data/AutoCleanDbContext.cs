using Microsoft.EntityFrameworkCore;
using Sistemas.Models;

namespace Sistemas.Data
{
    public class AutoCleanDbContext : DbContext
    {
        public AutoCleanDbContext(DbContextOptions<AutoCleanDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<HistorialServicio> Historiales { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<TipoServicio> TipoServicios { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<HistorialServicio>()
                .HasOne(h => h.Vehiculo)
                .WithMany()
                .HasForeignKey(h => h.IdVehiculo)
                .OnDelete(DeleteBehavior.Restrict);

            
        }
    }
}
