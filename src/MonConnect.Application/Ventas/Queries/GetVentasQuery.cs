using MediatR;

namespace MonConnect.Application.Ventas.Queries;

public class GetVentasQuery : IRequest<List<VentaDto>>
{
}
