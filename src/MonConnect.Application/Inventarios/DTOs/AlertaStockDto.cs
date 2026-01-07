
namespace MonConnect.Application.Inventarios.DTOs;

public class AlertaStockDto
{
    public Guid ProductoId {get; set;}
    public string ProductoNombre {get; set;} = null!;

    public Guid SucursalId {get; set;}
    public string SucursalNombre {get; set;} = null!;

    public decimal StockActual {get; set;}
    public decimal StockMinimo {get; set;}
}