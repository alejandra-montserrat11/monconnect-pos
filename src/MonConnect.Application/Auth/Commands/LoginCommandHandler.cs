
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwt;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IJwtTokenService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (usuario is null || !usuario.IsActivo)
            throw new UnauthorizedAccessException("Credenciales inválidas");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas");

        return _jwt.GenerarToken(usuario);
    }
}
