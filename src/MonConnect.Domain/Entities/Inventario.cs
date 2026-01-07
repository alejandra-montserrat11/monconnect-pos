
namespace MonConnect.Domain.Entities;

public class Inventario
{

    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;

    public Guid SucursalId { get; set; }
    public Sucursal Sucursal { get; set; } = null!;

    public decimal Existencia { get; set; }
    public decimal StockMinimo {get; set;}
} 