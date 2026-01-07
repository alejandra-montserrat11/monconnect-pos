
using MediatR;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Application.Inventarios.Commands;
using MonConnect.Domain.Entities;

namespace MonConnect.Application.Sucursales.Commands;

public class CreateSucursalCommandHandler : IRequestHandler<CreateSucursalCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateSucursalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateSucursalCommand request,
        CancellationToken cancellationToken)
    {
        var sucursal = new Sucursal
        {
            Nombre = request.Nombre,
            Direccion= request.Direccion,
        };

        _context.Sucursales.Add(sucursal);
        await _context.SaveChangesAsync(cancellationToken);

        return sucursal.Id;
    }
}