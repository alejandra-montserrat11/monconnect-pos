
namespace MonConnect.Domain.Entities;

public class Producto
{
    public Guid Id {get; set;} =Guid.NewGuid();
    public string Nombre {get; set;} = null!;
    public string Codigo {get; set;} = null!; //Ej: PISO-60X60-BLANCO
    public string Categoria {get; set;} = null!; // Piso, Azulejo, Adhesivo, Boquilla...
    public string Material {get; set; } = null!;  // Cerámica, Porcelanato, Piedra...
    public string Acabado {get; set;} = null!; // Mate, Brillante, etc
    public string Color {get; set;} = null!;

    public decimal PrecioPorMetroCuadrado {get; set;}
    public decimal PrecioPorCaja {get; set;}
    public decimal MetrosCuadradosPorCaja {get; set;}

    //activo o no activo dependiendo si se vende o no
    public bool Activo {get; set;} = true;

    //relacion con inventario
    public ICollection<Inventario>Inventarios {get; set;} = new List<Inventario>();
}