

using MediatR;
using System;

namespace MonConnect.Application.Sucursales.Commands;

public class CreateSucursalCommand : IRequest<Guid>
{
    public string Nombre{ get; set; } = null!;
    public string Direccion { get; set; } = null!;

}
