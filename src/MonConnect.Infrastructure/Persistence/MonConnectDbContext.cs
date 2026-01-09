using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;
using MonConnect.Domain.Constants; // Asegúrate de tener este using para Roles.Admin

namespace MonConnect.Infrastructure.Persistence;

public class MonConnectDbContext : DbContext, IApplicationDbContext
{
    public MonConnectDbContext(DbContextOptions<MonConnectDbContext> options)
        : base(options) { }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Sucursal> Sucursales { get; set; }
    public DbSet<Inventario> Inventarios { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<VentaDetalle> VentaDetalle { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Llamada base
        base.OnModelCreating(modelBuilder);

        // 2. Configuraciones de relaciones
        modelBuilder.Entity<Inventario>()
            .HasOne(i => i.Producto)
            .WithMany(p => p.Inventarios)
            .HasForeignKey(i => i.ProductoId);

        modelBuilder.Entity<Inventario>()
            .HasOne(i => i.Sucursal)
            .WithMany(s => s.Inventarios)
            .HasForeignKey(i => i.SucursalId);

        // 3. Sembrado de datos
        var adminId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");

        modelBuilder.Entity<Usuario>().HasData(new Usuario
        {
            Id = adminId,
            Email = "admin@monconnect.com",
            // IMPORTANTE: BCrypt generará un hash distinto cada vez que se cree una migración nueva
            // Para un sistema real, se suele poner el hash fijo, pero para probar esto sirve:
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
            Rol = Roles.Admin,
            IsActivo = true
        });
    }
}