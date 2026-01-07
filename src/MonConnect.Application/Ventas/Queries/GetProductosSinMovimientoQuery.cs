
using MediatR;
using MonConnect.Application.Ventas.DTOs;

public class GetProductosSinMovimientoQuery : IRequest<List<ProductoSinMovimientoDto>>
{
    public Guid? SucursalId {get; set;}
    public int Dias {get; set;} = 30; //configurable
} 