
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/dashboardGerencial")]
public class DashboardGerencialController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardGerencialController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Roles = $"{Roles.Admin},{Roles.Supervisor}")]
    [HttpGet("{sucursalId}")]
    public async Task<IActionResult> Get(Guid sucursalId)
    {
        var result = await _mediator.Send(
            new GetDashboardGerencialQuery { SucursalId = sucursalId }
        );

        return Ok(result);
    }
}
