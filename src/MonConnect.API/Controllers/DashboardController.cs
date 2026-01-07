


using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/dashboard")]

public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet ("{sucursalId}")]
    public async Task<IActionResult> Get(Guid sucursalId)
    {
        return Ok(await _mediator.Send(new GetDashboardPosQuery(sucursalId)));
    }

}