
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Ventas.DTOs;

public class GetVentasPorDiaQueryHandler
    : IRequestHandler<GetVentasPorDiaQuery, List<VentasPorDiaDto>>
{
    private readonly IApplicationDbContext _context;

    public GetVentasPorDiaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VentasPorDiaDto>> Handle(
        GetVentasPorDiaQuery request,
        CancellationToken cancellationToken)
    {
        var inicioUtc = DateTime.SpecifyKind(request.FechaInicio, DateTimeKind.Utc);
        var finUtc = DateTime.SpecifyKind(request.FechaFin, DateTimeKind.Utc);

        var query = _context.Ventas
            .Where(v =>
                v.Fecha >= inicioUtc &&
                v.Fecha <= finUtc
            );

        if (request.SucursalId.HasValue)
        {
            query = query.Where(v => v.SucursalId == request.SucursalId);
        }

        return await query
            .GroupBy(v => v.Fecha.Date)
            .Select(g => new VentasPorDiaDto
            {
                Fecha = g.Key,
                TotalVentas = g.Count(),
                TotalVendido = g.Sum(v => v.Total)
            })
            .OrderBy(d => d.Fecha)
            .ToListAsync(cancellationToken);
    }
}
