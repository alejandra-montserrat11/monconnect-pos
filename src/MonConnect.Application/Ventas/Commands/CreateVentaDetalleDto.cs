
namespace MonConnect.Application.Ventas.Commands;

public class CreateVentaDetalleDto
{
    public Guid ProductoId {get; set;}
    public int Cantidad {get; set;}
}