using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Ventas.DTOs;

public class GetCorteCajaQueryHandler
    : IRequestHandler<GetCorteCajaQuery, CorteCajaDto>
{
    private readonly IApplicationDbContext _context;

    public GetCorteCajaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CorteCajaDto> Handle(
        GetCorteCajaQuery request,
        CancellationToken cancellationToken)
    {
        var fechaInicio = request.Fecha.Date;
        var fechaFin = fechaInicio.AddDays(1);

        var fechaInicioUtc = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Utc);
        var fechaFinUtc = DateTime.SpecifyKind(fechaFin, DateTimeKind.Utc);
        
        var ventas = await _context.Ventas
            .Include(v => v.Sucursal)
            .Where(v =>
                v.SucursalId == request.SucursalId &&
                v.Fecha >= fechaInicioUtc &&
                v.Fecha < fechaFinUtc
            )
            .ToListAsync(cancellationToken);

        //CASO: NO HUBO VENTAS
        if (!ventas.Any())
        {
            var sucursal = await _context.Sucursales
                .Where(s => s.Id == request.SucursalId)
                .Select(s => s.Nombre)
                .FirstOrDefaultAsync(cancellationToken);

            return new CorteCajaDto
            {
                Fecha = fechaInicio,
                SucursalId = request.SucursalId,
                SucursalNombre = sucursal ?? "Sucursal no encontrada",
                TotalVentas = 0,
                TotalVendido = 0,
                Mensaje = "No se registraron ventas en esta fecha"
            };
        }

        var metodosPago = await _context.Pagos
            .Where(p =>
                p.Venta.SucursalId == request.SucursalId &&
                p.Venta.Fecha >= fechaInicioUtc &&
                p.Venta.Fecha < fechaFinUtc
            )
            .GroupBy(p => p.Metodo)
            .Select(g => new CorteCajaMetodoPagoDto
            {
                MetodoPago = g.Key,
                TotalVentas = g.Count(),
                TotalVendido = g.Sum(x => x.Monto)
            })
            .ToListAsync(cancellationToken);

        //CASO NORMAL
        return new CorteCajaDto
        {
            Fecha = fechaInicio,
            SucursalId = request.SucursalId,
            SucursalNombre = ventas.First().Sucursal.Nombre,
            TotalVentas = ventas.Count,
            TotalVendido = ventas.Sum(v => v.Total),
            MetodosPago = metodosPago,
            Mensaje = "Corte de caja generado correctamente"
        };
    }
}
