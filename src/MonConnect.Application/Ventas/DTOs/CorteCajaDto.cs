
public class CorteCajaDto
{
    public DateTime Fecha { get; set; }
    public Guid SucursalId { get; set; }
    public string SucursalNombre { get; set; } = null!;

    public int TotalVentas { get; set; }
    public decimal TotalVendido { get; set; }

    // 👇 NUEVO
    public string Mensaje { get; set; } = string.Empty;

    public List<CorteCajaMetodoPagoDto> MetodosPago { get; set; } = new();
}
