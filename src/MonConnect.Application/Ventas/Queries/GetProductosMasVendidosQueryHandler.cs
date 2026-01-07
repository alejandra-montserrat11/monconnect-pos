using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Ventas.DTOs;

using MonConnect.Application.Ventas.Queries;


public class GetProductosMasVendidosQueryHandler
    : IRequestHandler<GetProductosMasVendidosQuery, ProductosMasVendidosReporteDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductosMasVendidosQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductosMasVendidosReporteDto> Handle(
        GetProductosMasVendidosQuery request,
        CancellationToken cancellationToken
    )
    {
        var fechaInicioUtc = DateTime.SpecifyKind(
            request.FechaInicio.Date,
            DateTimeKind.Utc);

        var fechaFinUtc = DateTime.SpecifyKind(
            request.FechaFin.Date.AddDays(1),
            DateTimeKind.Utc);

        var query = _context.VentaDetalle
            .Include(d => d.Producto)
            .Include(d => d.Venta)
            .Where(d =>
                d.Venta.Fecha >= fechaInicioUtc &&
                d.Venta.Fecha < fechaFinUtc
            );

        if (request.SucursalId.HasValue)
        {
            query = query.Where(d => d.Venta.SucursalId == request.SucursalId);
        }

        var productos = await query
            .GroupBy(d => new
            {
                d.ProductoId,
                d.Producto.Nombre
            })
            .Select(g => new ProductoMasVendidoDto
            {
                ProductoId = g.Key.ProductoId,
                ProductoNombre = g.Key.Nombre,
                CantidadTotal = g.Sum(x => x.Cantidad),
                TotalVendido = g.Sum(x => x.Cantidad * x.PrecioUnitario)
            })
            .OrderByDescending(x => x.CantidadTotal)
            .ToListAsync(cancellationToken);

        // 🔴 CASO: NO HAY VENTAS
        if (!productos.Any())
        {
            return new ProductosMasVendidosReporteDto
            {
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                SucursalId = request.SucursalId,
                TotalProductos = 0,
                Productos = new List<ProductoMasVendidoDto>(),
                Mensaje = "No se registraron ventas en el periodo seleccionado"
            };
        }

        // 🟢 CASO NORMAL
        return new ProductosMasVendidosReporteDto
        {
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            SucursalId = request.SucursalId,
            TotalProductos = productos.Count,
            Productos = productos,
            Mensaje = "Reporte de productos más vendidos generado correctamente"
        };
    }
}
