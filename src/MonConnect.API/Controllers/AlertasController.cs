
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/alertas")]

public class AlertasController : ControllerBase
{
    private readonly IMediator _mediator;

    public AlertasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stock-bajo")]

    public async Task<IActionResult> GetStockBajo(
        [FromQuery] Guid? sucursalId)
    {
        var result = await _mediator.Send(
            new GetAlertasStockBajoQuery
            {
                SucursalId = sucursalId
            });
        if (!result.Any())
            return Ok("No hay alertas de stock bajo");

        return Ok(result);
    
    }
}