
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MonConnect.Application.Inventarios.Commands;

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
}