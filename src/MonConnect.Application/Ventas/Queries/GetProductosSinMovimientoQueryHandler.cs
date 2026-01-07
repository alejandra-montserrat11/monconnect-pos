using System.Security.Cryptography.X509Certificates;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Ventas.DTOs;
using MonConnect.Domain.Entities;


public class GetProductosSinMovimientoQueryHandler : IRequestHandler<GetProductosSinMovimientoQuery, List<ProductoSinMovimientoDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductosSinMovimientoQueryHandler(IApplicationDbContext context)
    { 
        _context = context;
    }

    public async Task<List<ProductoSinMovimientoDto>> Handle(
    GetProductosSinMovimientoQuery request,
    CancellationToken cancellationToken)
{
    var fechaLimite = DateTime.UtcNow.AddDays(-request.Dias);

    // Última venta por producto + sucursal (SQL SIMPLE)
    var ultimasVentas = await _context.VentaDetalle
        .Include(d => d.Venta)
        .GroupBy(d => new
        {
            d.ProductoId,
            d.Venta.SucursalId
        })
        .Select(g => new
        {
            g.Key.ProductoId,
            g.Key.SucursalId,
            UltimaVenta = g.Max(x => x.Venta.Fecha)
        })
        .ToListAsync(cancellationToken);

    // Inventarios
    var inventarios = await _context.Inventarios
        .Include(i => i.Producto)
        .Include(i => i.Sucursal)
        .Where(i =>
            !request.SucursalId.HasValue ||
            i.SucursalId == request.SucursalId
        )
        .ToListAsync(cancellationToken);

    // Lógica de negocio en memoria (LINQ normal)
    var resultado = inventarios
        .Select(i =>
        {
            var ultimaVenta = ultimasVentas
                .FirstOrDefault(v =>
                    v.ProductoId == i.ProductoId &&
                    v.SucursalId == i.SucursalId
                )?.UltimaVenta ?? DateTime.MinValue;

            return new ProductoSinMovimientoDto
            {
                ProductoId = i.ProductoId,
                ProductoNombre = i.Producto.Nombre,
                SucursalId = i.SucursalId,
                SucursalNombre = i.Sucursal.Nombre,
                //StockActual = i.Cantidad,
                UltimaVenta = ultimaVenta,
                DiasSinMovimiento = ultimaVenta == DateTime.MinValue
                    ? request.Dias
                    : (DateTime.UtcNow - ultimaVenta).Days
            };
        })
        .Where(x =>
            x.UltimaVenta == DateTime.MinValue ||
            x.UltimaVenta < fechaLimite
        )
        .ToList();

    return resultado;
}

}