
public class VentaRecienteDto
{
    public Guid VentaId {get; set;}
    public DateTime Fecha {get; set;}
    public decimal Total {get; set;}
    public string SucursalNombre {get; set;} = string.Empty;
}