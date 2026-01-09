using MediatR;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;
using MonConnect.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MonConnect.Application.Ventas.Commands;

public class CreateVentaCommandHandler : IRequestHandler<CreateVentaCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService; // <-- Nuevo servicio

    public CreateVentaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService; // <-- Inyectamos
    }

    public async Task<Guid> Handle(
        CreateVentaCommand request,
        CancellationToken cancellationToken)
    {
        var venta = new Venta
        {
            SucursalId = request.SucursalId,
            Fecha = DateTime.UtcNow
        };

        decimal total = 0;

        foreach (var item in request.Productos)
        {
            // 1. Buscamos el producto
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == item.ProductoId, cancellationToken);

            if (producto == null)
                throw new BusinessException($"El producto con ID {item.ProductoId} no existe.");

            if (!producto.Activo) 
                throw new BusinessException($"No se puede vender '{producto.Nombre}' porque está inactivo.");

            // 2. Validamos inventario
            var inventario = await _context.Inventarios
                .FirstOrDefaultAsync(i =>
                    i.ProductoId == item.ProductoId &&
                    i.SucursalId == request.SucursalId, cancellationToken);

            if (inventario == null || inventario.Existencia < item.Cantidad)
                throw new BusinessException($"Stock Insuficiente para el producto {producto.Nombre}");

            // 3. Descontamos stock
            inventario.Existencia -= item.Cantidad;

            // ---NUEVA LÓGICA DE MOVIMIENTO (AUDITORÍA) ---
            var movimiento = new Movimiento
            {
                Id = Guid.NewGuid(),
                ProductoId = producto.Id,
                SucursalId = request.SucursalId,
                UsuarioId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString()), // Rastreamos quién vende
                Tipo = "VENTA",
                Cantidad = item.Cantidad * -1, // Negativo porque es una salida
                Fecha = DateTime.UtcNow,
                Observacion = $"Venta realizada. Stock restante: {inventario.Existencia}"
            };
            _context.Movimientos.Add(movimiento);
            // --------------------------------------------------

            // 4. Creamos el detalle
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

        // 5. Validamos pagos
        var totalPagado = request.Pagos.Sum(p => p.Monto);
        if (totalPagado != venta.Total)
            throw new BusinessException("El total pagado no coincide con el total de la venta");

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