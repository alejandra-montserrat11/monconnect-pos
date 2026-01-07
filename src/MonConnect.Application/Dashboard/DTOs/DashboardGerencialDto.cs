
using MonConnect.Application.Ventas.DTOs;

namespace MonConnect.Application.Dashboard.DTOs;

public class DashboardGerencialDto
{
    public DateTime Fecha { get; set; }

    public Guid SucursalId { get; set; }
    public string SucursalNombre { get; set; } = string.Empty;

    public int TotalVentasHoy { get; set; }
    public decimal TotalVendidoHoy { get; set; }
    public decimal TicketPromedio { get; set; }

    public List<ProductoSinMovimientoDto> ProductosSinMovimiento { get; set; }
        = new();

    public List<ProductoStockBajoDto> ProductosStockBajo { get; set; }
        = new();
}
