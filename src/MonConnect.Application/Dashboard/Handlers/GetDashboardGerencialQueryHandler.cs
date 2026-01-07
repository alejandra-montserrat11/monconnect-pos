
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Dashboard.DTOs;
using MonConnect.Application.Ventas.Queries;

public class GetDashboardGerencialQueryHandler
    : IRequestHandler<GetDashboardGerencialQuery, DashboardGerencialDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetDashboardGerencialQueryHandler(
        IApplicationDbContext context,
        IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<DashboardGerencialDto> Handle(
        GetDashboardGerencialQuery request,
        CancellationToken cancellationToken)
    {
        var hoy = DateTime.UtcNow.Date;
        var mañana = hoy.AddDays(1);

        // 1️⃣ Ventas del día
        var ventasHoy = await _context.Ventas
            .Include(v => v.Sucursal)
            .Where(v =>
                v.SucursalId == request.SucursalId &&
                v.Fecha >= hoy &&
                v.Fecha < mañana
            )
            .ToListAsync(cancellationToken);

        // 2️⃣ Productos sin movimiento (REUSO)
        var productosSinMovimiento =
            await _mediator.Send(
                new GetProductosSinMovimientoQuery
                {
                    Dias = 30,
                    SucursalId = request.SucursalId
                },
                cancellationToken
            );

        // 3️⃣ Stock bajo (REUSO)
        //Stock bajo
        var productosStockBajo = await _context.Inventarios
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

        var totalVendido = ventasHoy.Sum(v => v.Total);
        var totalVentas = ventasHoy.Count;

        return new DashboardGerencialDto
        {
            Fecha = hoy,
            SucursalId = request.SucursalId,
            SucursalNombre =
                ventasHoy.FirstOrDefault()?.Sucursal.Nombre
                ?? "Sucursal",

            TotalVentasHoy = totalVentas,
            TotalVendidoHoy = totalVendido,
            TicketPromedio =
                totalVentas == 0
                    ? 0
                    : totalVendido / totalVentas,

            ProductosSinMovimiento = productosSinMovimiento,
            ProductosStockBajo = productosStockBajo
        };
    }
}
