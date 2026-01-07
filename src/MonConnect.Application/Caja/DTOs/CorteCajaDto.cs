
using MonConnect.Domain.Enums;

namespace MonConnect.Application.Caja.DTOs;

public class CorteCajaDto
{
    public MetodoPago Metodo { get; set; }
    public decimal Total { get; set; }
}
