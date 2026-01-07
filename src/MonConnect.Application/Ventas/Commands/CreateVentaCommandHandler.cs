using MediatR;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;

namespace MonConnect.Application.Ventas.Commands;

public class CreateVentaCommandHandler : IRequestHandler<CreateVentaCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateVentaCommandHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateVentaCommand request,
        CancellationToken cancellationToken)

    {
        var venta = new Venta

        {
            SucursalId = request.SucursalId
        };

        decimal total = 0;

        foreach(var item in request.Productos)
        {
            var inventario = _context.Inventarios
                .FirstOrDefault(i =>
                    i.ProductoId == item.ProductoId &&
                    i.SucursalId == request.SucursalId);

            if (inventario == null || inventario.Existencia < item.Cantidad) 
               throw new Exception("Stock Insuficiente");

            inventario.Existencia -= item.Cantidad;

            var producto = _context.Productos.First(p => p.Id == item.ProductoId);

            var detalle = new VentaDetalle
            {
                ProductoId = producto.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.PrecioPorCaja,
                Subtotal = producto.PrecioPorCaja * item.Cantidad
            };

            total += detalle.Subtotal;
            venta.Detalles.Add(detalle);

        }

        venta.Total = total;

        var totalPagado = request.Pagos.Sum(p => p.Monto);
        if (totalPagado != venta.Total)
        throw new Exception("El total pagado no coincide con el total de la venta");

        foreach (var pago in request.Pagos)
        {
            venta.Pagos.Add(new Pago
            {
                Metodo = pago.Metodo,
                Monto = pago.Monto
            });
        }

        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync(cancellationToken);

        return venta.Id;
    }
}