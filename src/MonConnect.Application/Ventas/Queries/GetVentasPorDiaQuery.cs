
using MediatR;
using MonConnect.Application.Ventas.DTOs;

public class GetVentasPorDiaQuery : IRequest<List<VentasPorDiaDto>>
{
    public DateTime FechaInicio {get; set;}
    public DateTime FechaFin {get; set;}
    public Guid? SucursalId {get; set;}
}