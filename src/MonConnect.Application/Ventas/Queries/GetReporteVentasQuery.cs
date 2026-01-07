
using MediatR;

namespace MonConnect.Application.Ventas.Queries;

public class GetReporteVentasQuery : IRequest<ReporteVentasDto>
{
    public DateTime FechaInicio {get; set;}
    public DateTime FechaFin{get; set;}
    public Guid? SucursalId {get; set;}
}