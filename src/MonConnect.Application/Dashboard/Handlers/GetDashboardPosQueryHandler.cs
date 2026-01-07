
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Dashboard.DTOs;
using MonConnect.Application.Ventas.DTOs;
//using MonConnect.Application.Dashboard.Queries;

public class GetDashboardPosQueryHandler : IRequestHandler<GetDashboardPosQuery, DashboardPosDto>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardPosQueryHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardPosDto> Handle(
        GetDashboardPosQuery request,
        CancellationToken cancellationToken
    )
    {
        var hoyUtc = DateTime.UtcNow.Date;
        var MañanaUtc = hoyUtc.AddDays(1);

        //Ventas del dia
        var ventasHoy = await _context.Ventas
            .Where( v =>
                v.SucursalId == request.SucursalId &&
                v.Fecha >= hoyUtc &&
                v.Fecha < MañanaUtc
                )
                .ToListAsync(cancellationToken);

        //Productos mas vendidos hoy
        var productosMasVendidos = await _context.VentaDetalle
            .Include(d => d.Producto)
            .Include(d => d.Venta)
            .Where(d =>
                d.Venta.SucursalId == request.SucursalId &&
                d.Venta.Fecha >= hoyUtc &&
                d.Venta.Fecha < MañanaUtc
                )
                .GroupBy(d => new {d.ProductoId, d.Producto.Nombre})
                .Select(g => new ProductoMasVendidoDto
                {
                    ProductoId = g.Key.ProductoId,
                    ProductoNombre = g.Key.Nombre,
                    CantidadTotal = g.Sum( x => x.Cantidad),
                    TotalVendido = g.Sum( x => x.Cantidad * x.PrecioUnitario)
                })
                .OrderByDescending(x => x.CantidadTotal)
                .Take(5)
                .ToListAsync(cancellationToken);

        //Stock bajo
        var stockBajo = await _context.Inventarios
            .Include(i => i.Producto)
            .Where( i =>
                i.SucursalId == request.SucursalId &&
                i.Existencia <=10
                )
                .Select(i => new ProductoStockBajoDto
                {
                    ProductoId = i.ProductoId,
                    ProductoNombre = i.Producto.Nombre,
                    StockActual = i.Existencia
                })
                .ToListAsync(cancellationToken);

        // Ventas recientes
        var ventasRecientes = await _context.Ventas
            .Include(v => v.Sucursal)
            .Where(v => v.SucursalId == request.SucursalId)
            .OrderByDescending(v => v.Fecha)
            .Take(5)
            .Select(v => new VentaRecienteDto
            {
                VentaId = v.Id,
                Fecha = v.Fecha,
                Total = v.Total,
                SucursalNombre = v.Sucursal.Nombre
            })
            .ToListAsync(cancellationToken);

         return new DashboardPosDto
        {
            VentasHoy = ventasHoy.Count,
            TotalVendidoHoy = ventasHoy.Sum(v => v.Total),
            ProductosMasVendidos = productosMasVendidos,
            ProductosStockBajo = stockBajo,
            VentasRecientes = ventasRecientes
        };

    }
}