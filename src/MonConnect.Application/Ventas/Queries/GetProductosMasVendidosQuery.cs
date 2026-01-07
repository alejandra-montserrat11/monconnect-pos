
using MediatR;

using MonConnect.Application.Ventas.DTOs;

namespace MonConnect.Application.Ventas.Queries;


public class GetProductosMasVendidosQuery : IRequest<ProductosMasVendidosReporteDto>
{
    public DateTime FechaInicio {get; set;}
    public DateTime FechaFin {get; set;}
    public Guid? SucursalId {get; set;}
}