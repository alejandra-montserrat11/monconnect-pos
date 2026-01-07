
using MediatR;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;

namespace MonConnect.Application.Inventarios.Commands;

public class AddInventarioCommandHandler : IRequestHandler<AddInventarioCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public AddInventarioCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        AddInventarioCommand request,
        CancellationToken cancellationToken)
    {
        var inventario = new Inventario
        {
            ProductoId = request.ProductoId,
            SucursalId = request.SucursalId,
            Existencia = request.Existencia
        };

        _context.Inventarios.Add(inventario);
        await _context.SaveChangesAsync(cancellationToken);

        return inventario.Id;
    }
}