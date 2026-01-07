
namespace MonConnect.Domain.Entities;

public class Sucursal
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public string Nombre{get; set;} = null!;
    public string Direccion {get; set;} = null!;

    public ICollection<Inventario> Inventarios {get; set;} = new List<Inventario>();

    public bool IsActivo { get; set; } = true;
    public DateTime? FechaDesactivado { get; set; }

}