
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;

namespace MonConnect.Application.Usuarios.Queries;

// Definimos qué datos queremos mostrar (sin el PasswordHash)
public record UsuarioDto(Guid Id, string Email, string Rol, bool IsActivo);

public record GetUsuariosQuery() : IRequest<List<UsuarioDto>>;

public class GetUsuariosQueryHandler : IRequestHandler<GetUsuariosQuery, List<UsuarioDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUsuariosQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UsuarioDto>> Handle(GetUsuariosQuery request, CancellationToken cancellationToken)
    {
        return await _context.Usuarios
            .Select(u => new UsuarioDto(
                u.Id,
                u.Email,
                u.Rol,
                u.IsActivo))
            .ToListAsync(cancellationToken);
    }
}