
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MonConnect.Application.Ventas.Commands;
using MonConnect.Application.Ventas.Queries;
using Microsoft.AspNetCore.Authorization;

namespace MonConnect.API.Controllers;

[ApiController]
[Route("api/ventas")]

public class VentasController : ControllerBase
{
    private readonly IMediator _mediator;

    public VentasController (IMediator mediator)
    {
        _mediator = mediator;
    }

    //Registrar una venta
    [Authorize(Roles = $"{Roles.Admin},{Roles.Cajero}")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateVentaCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }
    
    
    // Obtener las ventas realizadas

    [HttpGet]
    
    public async Task<IActionResult> Get()
    {
        var ventas = await _mediator.Send(new GetVentasQuery());
        return Ok(ventas);
        
    }


    //Endpoint para obtener el reporte de ventas en cada sucursal con filtrado de fechas y sucursal

    [HttpGet("reporte")]

    public async Task<IActionResult> GetReporte(
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin,
        [FromQuery] Guid? sucursalId)
    {
        var reporte = await _mediator.Send(new GetReporteVentasQuery
        {
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            SucursalId = sucursalId
        });

        return Ok(reporte);
    }

    //endpoint para obtener los productos mas vendidos con filtrado de fecha y sucursal

    [HttpGet("productos-mas-vendidos")]
    
    public async Task<IActionResult> GetProductosMasVendidos(
    [FromQuery] DateTime fechaInicio,
    [FromQuery] DateTime fechaFin,
    [FromQuery] Guid? sucursalId)
    {
    var result = await _mediator.Send(
        new GetProductosMasVendidosQuery
        {
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            SucursalId = sucursalId
        });

    return Ok(result);
    
    }


    //Endpoint para calcular el corte de caja
    [HttpGet("corte-caja")]
    public async Task<ActionResult> GetCorteCaja(
        [FromQuery] DateTime fecha,
        [FromQuery] Guid sucursalId)
    {
        
        var result = await _mediator.Send(new GetCorteCajaQuery
        {
            Fecha = fecha,
            SucursalId = sucursalId
        });

        return Ok(result);
    }

    [HttpGet("corte-caja/metodos-pago")]
public async Task<IActionResult> GetCorteCajaPorMetodoPago(
    [FromQuery] Guid sucursalId,
    [FromQuery] DateTime fecha)
{
    var result = await _mediator.Send(
        new GetCorteCajaPorMetodoPagoQuery(sucursalId, fecha));

    return Ok(result);
}

    

}