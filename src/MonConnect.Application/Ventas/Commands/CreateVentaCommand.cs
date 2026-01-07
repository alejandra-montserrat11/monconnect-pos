
using MediatR;
using MonConnect.Application.Pagos.DTOs;

namespace MonConnect.Application.Ventas.Commands;

public class CreateVentaCommand : IRequest<Guid>
{
    public Guid SucursalId {get; set;}
    public List<CreateVentaDetalleDto> Productos {get; set;} = new();

    public List<PagoDto> Pagos { get; set; } = new();
}