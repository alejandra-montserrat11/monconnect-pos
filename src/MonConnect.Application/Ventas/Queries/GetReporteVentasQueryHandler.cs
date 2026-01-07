using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Ventas.Queries;
using MonConnect.Application.Ventas.DTOs;

namespace MonConnect.Application.Ventas.DTOs;

public class GetReporteVentasQueryHandler
    : IRequestHandler<GetReporteVentasQuery, ReporteVentasDto>
{
    private readonly IApplicationDbContext _context;

    public GetReporteVentasQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReporteVentasDto> Handle(
        GetReporteVentasQuery request,
        CancellationToken cancellationToken
    )
    {
        var fechaInicioUtc = DateTime.SpecifyKind(
            request.FechaInicio.Date,
            DateTimeKind.Utc);

        var fechaFinUtc = DateTime.SpecifyKind(
            request.FechaFin.Date.AddDays(1),
            DateTimeKind.Utc);

        var query = _context.Ventas
            .Include(v => v.Sucursal)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .Where(v =>
                v.Fecha >= fechaInicioUtc &&
                v.Fecha < fechaFinUtc
            );

        if (request.SucursalId.HasValue)
        {
            query = query.Where(v => v.SucursalId == request.SucursalId);
        }

        var ventas = await query.ToListAsync(cancellationToken);

        // 🔴 CASO: NO HAY VENTAS
        if (!ventas.Any())
        {
            string? sucursalNombre = null;

            if (request.SucursalId.HasValue)
            {
                sucursalNombre = await _context.Sucursales
                    .Where(s => s.Id == request.SucursalId.Value)
                    .Select(s => s.Nombre)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            return new ReporteVentasDto
            {
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                SucursalId = request.SucursalId,
                SucursalNombre = sucursalNombre,
                TotalVentas = 0,
                MontoTotal = 0,
                Ventas = new List<VentaDto>(),
                Mensaje = "No se registraron ventas en el periodo seleccionado"
            };
        }

        // 🟢 CASO NORMAL
        return new ReporteVentasDto
        {
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            SucursalId = request.SucursalId,
            SucursalNombre = ventas.First().Sucursal.Nombre,
            TotalVentas = ventas.Count,
            MontoTotal = ventas.Sum(v => v.Total),
            Ventas = ventas.Select(v => new VentaDto
            {
                Id = v.Id,
                Fecha = v.Fecha,
                Total = v.Total,
                SucursalId = v.SucursalId,
                SucursalNombre = v.Sucursal.Nombre,
                Detalles = v.Detalles.Select(d => new VentaDetalleDto
                {
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            }).ToList(),
            Mensaje = "Reporte de ventas generado correctamente"
        };
    }
}
