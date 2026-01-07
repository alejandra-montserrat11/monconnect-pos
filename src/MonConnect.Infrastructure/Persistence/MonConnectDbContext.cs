using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;

namespace MonConnect.Infrastructure.Persistence;

public class MonConnectDbContext 
    : DbContext, IApplicationDbContext
{
    public MonConnectDbContext(DbContextOptions<MonConnectDbContext> options)
        : base(options) {}

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Sucursal> Sucursales { get; set; }
    public DbSet<Inventario> Inventarios { get; set; }

    public DbSet<Venta> Ventas{ get; set;}

    public DbSet<VentaDetalle> VentaDetalle{ get; set;}

    public DbSet<Pago> Pagos{ get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Inventario>()
            .HasOne(i => i.Producto)
            .WithMany(p => p.Inventarios)
            .HasForeignKey(i => i.ProductoId);

        modelBuilder.Entity<Inventario>()
            .HasOne(i => i.Sucursal)
            .WithMany(s => s.Inventarios)
            .HasForeignKey(i => i.SucursalId);
    }
}
