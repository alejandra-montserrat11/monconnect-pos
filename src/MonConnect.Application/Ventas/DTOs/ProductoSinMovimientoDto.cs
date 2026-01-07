
namespace MonConnect.Application.Ventas.DTOs;

public class ProductoSinMovimientoDto
{
    public Guid ProductoId {get; set;}
    public string ProductoNombre {get; set;} = null!;

    public Guid SucursalId {get; set;}
    public string SucursalNombre {get; set;} = null!;

    public DateTime? UltimaVenta {get; set;}
    public int DiasSinMovimiento {get; set;}
}