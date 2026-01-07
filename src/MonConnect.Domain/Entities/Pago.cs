
using MonConnect.Domain.Enums;

namespace MonConnect.Domain.Entities;

public class Pago
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid VentaId { get; set; }
    public MetodoPago Metodo { get; set; }
    public decimal Monto { get; set; }

    public Venta Venta { get; set; } = null!;
}
