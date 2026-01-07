
public class ReporteVentasDto
{
    public DateTime FechaInicio {get; set;}
    public DateTime FechaFin {get; set;}

    public Guid? SucursalId {get; set;}
    public string? SucursalNombre {get; set;}

    public int TotalVentas {get; set;}
    public decimal MontoTotal {get; set;}

    public string Mensaje { get; set; } = string.Empty;

    public List<VentaDto> Ventas {get; set;} = new();
}