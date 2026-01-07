
using MediatR;
using MonConnect.Application.Ventas.DTOs;

public class GetCorteCajaQuery : IRequest<CorteCajaDto>
{
    public DateTime Fecha { get; set; }
    public Guid SucursalId { get; set; }
}
