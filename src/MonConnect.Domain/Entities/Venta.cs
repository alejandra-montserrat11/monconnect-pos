
namespace MonConnect.Domain.Entities;

public class Venta
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public Guid SucursalId {get; set;}
    public DateTime Fecha  {get; set;} = DateTime.UtcNow;
    public decimal Total {get; set;}

    public Sucursal Sucursal {get; set;} = null!;
    public ICollection<VentaDetalle> Detalles{get; set;} = new List<VentaDetalle>();
    
    
    //Para registrar el metodo de pago
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();

}