
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;

namespace MonConnect.Application.Ventas.DTOs;

public class GetCorteCajaPorMetodoPagoHandler
    : IRequestHandler<GetCorteCajaPorMetodoPagoQuery, List<CorteCajaMetodoPagoDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCorteCajaPorMetodoPagoHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CorteCajaMetodoPagoDto>> Handle(
    GetCorteCajaPorMetodoPagoQuery request,
    CancellationToken cancellationToken)
{
    var fechaInicio = request.Fecha.Date;
    var fechaFin = fechaInicio.AddDays(1);

    var inicioUtc = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Utc);
    var finUtc = DateTime.SpecifyKind(fechaFin, DateTimeKind.Utc);

    return await _context.Ventas
    .Where(v =>
        v.SucursalId == request.SucursalId &&
        v.Fecha >= inicioUtc &&
        v.Fecha < finUtc
    )
    .SelectMany(
        v => v.Pagos,
        (venta, pago) => new
        {
            venta.Id,
            venta.Total,
            MetodoPago = pago.Metodo
        }
    )
    .GroupBy(x => x.MetodoPago)
    .Select(g => new CorteCajaMetodoPagoDto
    {
        MetodoPago = g.Key,
        TotalVentas = g.Count(),
        TotalVendido = g.Sum(x => x.Total)
    })
    .ToListAsync(cancellationToken);

}
}
