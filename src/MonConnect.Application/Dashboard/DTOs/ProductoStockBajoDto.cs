
public class ProductoStockBajoDto
{
    public Guid ProductoId {get; set;}
    public string ProductoNombre {get; set;} = string.Empty;
    public decimal StockActual {get; set;}
}