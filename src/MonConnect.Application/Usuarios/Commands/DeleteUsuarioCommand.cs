
using MediatR;
using MonConnect.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MonConnect.Domain.Entities;
using MonConnect.Domain.Constants;

namespace MonConnect.Application.Usuarios.Commands;

public record DesactivarUsuarioCommand(Guid Id) : IRequest<Unit>;

public class DesactivarUsuarioCommandHandler : IRequestHandler<DesactivarUsuarioCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public DesactivarUsuarioCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Unit> Handle(DesactivarUsuarioCommand request, CancellationToken ct)
    {
        var usuario = await _context.Usuarios.FindAsync(new object[] { request.Id }, ct);
        
        if (usuario == null) throw new Exception("Usuario no encontrado");

        usuario.IsActivo = false; // Solo lo desactivamos
        await _context.SaveChangesAsync(ct);
        
        return Unit.Value;
    }
}