
public class ProductosMasVendidosReporteDto
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public Guid? SucursalId { get; set; }

    public int TotalProductos { get; set; }
    public List<ProductoMasVendidoDto> Productos { get; set; } = new();
    public string Mensaje { get; set; } = string.Empty;
}
