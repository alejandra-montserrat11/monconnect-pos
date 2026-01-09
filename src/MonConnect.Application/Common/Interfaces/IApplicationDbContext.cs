
using Microsoft.EntityFrameworkCore;
using MonConnect.Domain.Entities;


namespace MonConnect.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Producto> Productos { get; }

        DbSet<Sucursal> Sucursales { get; }
        DbSet<Inventario> Inventarios { get; }

        DbSet<Venta> Ventas { get; }
        DbSet<VentaDetalle> VentaDetalle { get; }

        DbSet<Pago> Pagos { get; }

        DbSet<Usuario> Usuarios { get; }

        DbSet<Movimiento> Movimientos { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
