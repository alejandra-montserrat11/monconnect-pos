
namespace MonConnect.Domain.Entities;

public class Movimiento
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductoId { get; set; }
    public Guid SucursalId { get; set; }
    public Guid UsuarioId { get; set; }
    
    public string Tipo { get; set; } = string.Empty; // VENTA, AJUSTE, ENTRADA
    public int Cantidad { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string? Observacion { get; set; }
    public string UsuarioNombre {get; set;} = string.Empty;

    // Propiedades de navegación
    public Producto Producto { get; set; } = null!;
    public Sucursal Sucursal { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}