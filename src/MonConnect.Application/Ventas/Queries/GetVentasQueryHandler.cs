using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;

namespace MonConnect.Application.Ventas.Queries;

public class GetVentasQueryHandler
    : IRequestHandler<GetVentasQuery, List<VentaDto>>
{
    private readonly IApplicationDbContext _context;

    public GetVentasQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VentaDto>> Handle(
        GetVentasQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Ventas
            .Include(v => v.Sucursal)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .Select(v => new VentaDto
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
            })
            .ToListAsync(cancellationToken);
    }
}
