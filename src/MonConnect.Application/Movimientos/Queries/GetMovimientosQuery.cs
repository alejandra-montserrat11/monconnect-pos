using MediatR;
using Microsoft.EntityFrameworkCore; 
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;

namespace MonConnect.Application.Movimientos.Queries;

// 1. Definimos la estructura de la consulta (lo que pides)
public record GetMovimientosQuery() : IRequest<List<MovimientoDto>>;

// 2. Definimos qué datos queremos mostrar (DTO)
public record MovimientoDto(
    DateTime Fecha, 
    string Tipo, 
    int Cantidad, 
    string ProductoNombre, 
    string UsuarioEmail);

// 3. El manejador de la lógica
public class GetMovimientosQueryHandler : IRequestHandler<GetMovimientosQuery, List<MovimientoDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMovimientosQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovimientoDto>> Handle(GetMovimientosQuery request, CancellationToken ct)
    {
        return await _context.Movimientos
            .Include(m => m.Producto) // Carga los datos del producto
            .Include(m => m.Usuario)  // Carga los datos del usuario
            .OrderByDescending(m => m.Fecha)
            .Select(m => new MovimientoDto(
                m.Fecha, 
                m.Tipo, 
                m.Cantidad, 
                m.Producto.Nombre, 
                m.Usuario.Email))
            .ToListAsync(ct);
    }
}