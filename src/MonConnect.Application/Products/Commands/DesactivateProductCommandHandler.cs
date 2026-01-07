using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;

public class DeleteProductoCommandHandler
    : IRequestHandler<DeleteProductoCommand, string>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(
        DeleteProductoCommand request,
        CancellationToken cancellationToken)
    {
        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (producto == null)
            return "Producto no encontrado";

        // 🔒 VALIDACIÓN: ¿tiene ventas?
        var tieneVentas = await _context.VentaDetalle
            .AnyAsync(d => d.ProductoId == request.Id, cancellationToken);

        if (tieneVentas)
            return "No se puede eliminar el producto porque tiene ventas registradas";

        producto.Activo = false;

        await _context.SaveChangesAsync(cancellationToken);

        return "Producto eliminado correctamente";
    }
}
