
public class ProductoMasVendidoDto
{
    public Guid ProductoId {get; set;}
    public string ProductoNombre {get; set;} = null!;
    public decimal CantidadTotal {get; set;}
    public decimal TotalVendido {get; set;}
}