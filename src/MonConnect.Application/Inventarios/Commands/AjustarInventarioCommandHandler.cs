using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;

namespace MonConnect.Application.Inventarios.Commands;

public class AjustarInventarioCommandHandler : IRequestHandler<AjustarInventarioCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService; // Para la auditoría automática

    public AjustarInventarioCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(
        AjustarInventarioCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Buscar el inventario
        var inventario = await _context.Inventarios
            .FirstOrDefaultAsync(i =>
               i.ProductoId == request.ProductoId &&
               i.SucursalId == request.SucursalId,
               cancellationToken);

        if (inventario == null)
            throw new Exception("No existe registro de inventario para este producto en esta sucursal.");

        // 2. Aplicar el ajuste
        var nuevoStock = inventario.Existencia + request.CantidadAjuste;

        if (nuevoStock < 0)
            throw new Exception("Error: El ajuste no puede dejar el stock en valores negativos.");

        inventario.Existencia = nuevoStock;

        // 3. Registrar movimiento en la tabla de Auditoría
        _context.Movimientos.Add(new Movimiento
        {
            Id = Guid.NewGuid(),
            ProductoId = request.ProductoId,
            SucursalId = request.SucursalId,
            Tipo = "AJUSTE",
            Cantidad = request.CantidadAjuste,
            // Obtenemos el ID del usuario del Token
            UsuarioId = Guid.Parse(_currentUserService.UserId ?? Guid.Empty.ToString()), 
            Observacion = request.Motivo,
            Fecha = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value; // Indica que la tarea terminó correctamente
    }
}