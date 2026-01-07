
using MonConnect.Domain.Enums;
namespace MonConnect.Application.Pagos.DTOs;

public class PagoDto
{
    public MetodoPago Metodo { get; set; }
    public decimal Monto { get; set; }
}
