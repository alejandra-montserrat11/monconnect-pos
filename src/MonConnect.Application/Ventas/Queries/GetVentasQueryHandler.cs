using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Common.Exceptions;

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
        // 1. Obtenemos las ventas incluyendo los productos para poder validar
        var ventas = await _context.Ventas
            .Include(v => v.Sucursal)
            .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
            .ToListAsync(cancellationToken);

        // 2. Validamos si hay algún producto inactivo en las ventas obtenidas
        foreach (var venta in ventas)
        {
            foreach (var detalle in venta.Detalles)
            {
                if (!detalle.Producto.Activo)
                {
                    throw new BusinessException(
                        $"El producto {detalle.Producto.Nombre} está inactivo"
                    );
                }
            }
        }

        // 3. Mapeamos a DTO y retornamos
        return ventas.Select(v => new VentaDto
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
        }).ToList();
    }
}