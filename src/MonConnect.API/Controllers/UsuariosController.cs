
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MonConnect.Application.Usuarios.Commands;
using MonConnect.Domain.Constants;

using MonConnect.Application.Usuarios.Queries;

namespace MonConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)] // Solo administradores pueden gestionar usuarios
public class UsuariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuariosController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateUsuarioCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<ActionResult> Desactivar(Guid id)
    {
    await _mediator.Send(new DesactivarUsuarioCommand(id));
    return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {

    return Ok("Usuario eliminado");
    }

    [HttpGet]
    public async Task<ActionResult<List<UsuarioDto>>> GetAll()
    {
        var usuarios = await _mediator.Send(new GetUsuariosQuery());
        return Ok(usuarios);
    }
}