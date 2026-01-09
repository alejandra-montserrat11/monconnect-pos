using MediatR;

namespace MonConnect.Application.Inventarios.Commands;

public class AjustarInventarioCommand : IRequest<Unit> // <--- ESTO ES LO QUE FALTA
{
    public Guid ProductoId { get; set; }
    public Guid SucursalId { get; set; }
    public int CantidadAjuste { get; set; } 
    public string Motivo { get; set; } = string.Empty;

}