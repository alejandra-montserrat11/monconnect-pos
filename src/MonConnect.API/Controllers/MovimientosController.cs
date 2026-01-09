using MediatR;
using Microsoft.AspNetCore.Mvc;
using MonConnect.Application.Inventarios.Commands;
using Microsoft.AspNetCore.Authorization;
using MonConnect.Domain.Constants;
using MonConnect.Application.Movimientos.Queries;

namespace MonConnect.API.Controllers;

[Authorize(Roles = Roles.Admin)]
[ApiController]
[Route("api/[controller]")]
public class MovimientosController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovimientosController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task <ActionResult<List<MovimientoDto>>> Get()
    {
        return await _mediator.Send(new GetMovimientosQuery());
    }
}