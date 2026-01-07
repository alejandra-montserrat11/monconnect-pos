
public class VentaDto
{
    public Guid Id { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }

    public Guid SucursalId { get; set; }
    public string SucursalNombre { get; set; } = null!;

    public List<VentaDetalleDto> Detalles { get; set; } = new();
}
