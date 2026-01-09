
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MonConnect.Application.Inventarios.Commands;
using Microsoft.AspNetCore.Authorization;
using MonConnect.Domain.Constants;

namespace MonConnect.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventariosController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventariosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddInventarioCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [Authorize(Roles = "Admin,Supervisor")]
    [HttpPost("ajuste")]
    public async Task<IActionResult> AjustarInventario(
    AjustarInventarioCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { mensaje = "Ajuste de inventario realizado correctamente" });
        }
}