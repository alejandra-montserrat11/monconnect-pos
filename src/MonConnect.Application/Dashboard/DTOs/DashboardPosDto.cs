
namespace MonConnect.Application.Dashboard.DTOs;

public class DashboardPosDto
{
    public int VentasHoy {get; set;}
    public decimal TotalVendidoHoy {get; set;}

    public List<ProductoMasVendidoDto> ProductosMasVendidos {get; set; } = new();
    public List<ProductoStockBajoDto> ProductosStockBajo {get; set;} = new();
    public List<VentaRecienteDto> VentasRecientes {get; set;} = new();

    
}