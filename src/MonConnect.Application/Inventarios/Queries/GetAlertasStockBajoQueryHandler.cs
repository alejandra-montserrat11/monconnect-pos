using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Inventarios.DTOs;

public class GetAlertasStockBajoQueryHandler : IRequestHandler<GetAlertasStockBajoQuery, List<AlertaStockDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAlertasStockBajoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AlertaStockDto>> Handle(
        GetAlertasStockBajoQuery request,
        CancellationToken cancellationToken
    )
    {
        var query = _context.Inventarios
            .Include(i => i.Producto)
            .Include(i => i.Sucursal)
            .Where(i=> i.Existencia <= i.StockMinimo);

        if (request.SucursalId.HasValue)
        {
            query=query.Where(i=> i.SucursalId == request.SucursalId);
        }

        return await query
            .Select(i => new AlertaStockDto
            {
                ProductoId = i.ProductoId,
                ProductoNombre = i.Producto.Nombre,
                SucursalId = i.SucursalId,
                SucursalNombre = i.Sucursal.Nombre,
                StockActual = i.Existencia,
                StockMinimo= i.StockMinimo
            })
            .ToListAsync(cancellationToken);
    }
}