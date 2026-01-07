
using MediatR;
using System;

namespace MonConnect.Application.Inventarios.Commands;

public class AddInventarioCommand : IRequest<Guid>
{
    public Guid ProductoId { get; set; }
    public Guid SucursalId { get; set; }
    public decimal Existencia { get; set; }
}
