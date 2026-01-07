
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MonConnect.Application.Sucursales.Commands;

namespace MonConnect.API.Controllers;

[ApiController]
[Route("api/sucursales")]
public class SucursalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SucursalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSucursalCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }
}
