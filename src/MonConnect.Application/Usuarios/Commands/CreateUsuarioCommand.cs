using MediatR;
using MonConnect.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MonConnect.Domain.Entities;
using MonConnect.Domain.Constants;

namespace MonConnect.Application.Usuarios.Commands;

public record CreateUsuarioCommand(
    string Email,
    string Password,
    string Rol
) : IRequest<Guid>;

public class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateUsuarioCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {

        // 1. Verificar si el email ya existe
    var existe = await _context.Usuarios
        .AnyAsync(u => u.Email == request.Email, cancellationToken);

    if (existe)
    {
        throw new Exception("El correo electrónico ya está registrado.");
    }  
        // 1. Encriptar la contraseña con BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 2. Crear la entidad
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = passwordHash,
            Rol = request.Rol,
            IsActivo = true
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync(cancellationToken);

        return usuario.Id;
    }
}