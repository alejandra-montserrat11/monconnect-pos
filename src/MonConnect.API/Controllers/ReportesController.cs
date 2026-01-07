
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MonConnect.Application.Ventas.Queries;
using Microsoft.AspNetCore.Authorization;

namespace MonConnect.API.Controllers;

[ApiController]
[Route("api/Reportes")]
public class ReportesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPdfGenerator _pdfGenerator;

    public ReportesController(IMediator mediator, IPdfGenerator pdfGenerator)
    {
        _mediator = mediator;
        _pdfGenerator = pdfGenerator;
    }

    [HttpGet("productos-sin-movimiento")]
public async Task<IActionResult> GetProductosSinMovimiento(
    [FromQuery] int dias = 30,
    [FromQuery] Guid? sucursalId = null)
{
    var result = await _mediator.Send(
        new GetProductosSinMovimientoQuery
        {
            Dias = dias,
            SucursalId = sucursalId
        });

    if (!result.Any())
        return Ok("Todos los productos tienen movimiento");

    return Ok(result);
}

[HttpGet("ventas-por-dia")]
public async Task<IActionResult> GetVentasPorDia(
    DateTime fechaInicio,
    DateTime fechaFin,
    Guid? sucursalId)
{
    var result = await _mediator.Send(new GetVentasPorDiaQuery
    {
        FechaInicio = fechaInicio,
        FechaFin = fechaFin,
        SucursalId = sucursalId
    });

    return Ok(result);
}

[Authorize(Policy = Policies.PuedeExportarReportes)]
[HttpGet("export/ventas/excel")]
public async Task<IActionResult> ExportVentasExcel(
    DateTime fechaInicio,
    DateTime fechaFin,
    Guid? sucursalId
)
{
    var file = await _mediator.Send(
        new ExportReporteVentasExcelQuery(
            fechaInicio,
            fechaFin,
            sucursalId
        )
    );

    return File(
        file.Content,
        file.ContentType,
        file.FileName
    );
}

[Authorize(Policy = Policies.PuedeExportarReportes)]
[HttpGet("export/corte-caja/excel")]
public async Task<IActionResult> ExportCorteCajaExcel(
    DateTime fecha,
    Guid sucursalId
)
{
    var file = await _mediator.Send(
        new ExportCorteCajaExcelQuery(fecha, sucursalId)
    );

    return File(
        file.Content,
        file.ContentType,
        file.FileName
    );
}

[Authorize(Policy = Policies.PuedeExportarReportes)]
//enpoint para exportar excel de productos sin movimiento
[HttpGet("export/productos-sin-movimiento/excel")]
public async Task<IActionResult> ExportProductosSinMovimientoExcel(
    int dias,
    Guid? sucursalId
)
{
    var file = await _mediator.Send(
        new ExportProductosSinMovimientoExcelQuery(dias, sucursalId)
    );

    return File(
        file.Content,
        file.ContentType,
        file.FileName
    );
}

//generar pdf con informacion sobre el corte de caja
[Authorize(Policy = Policies.PuedeExportarReportes)]
    [HttpGet("reporte-ejecutivo/pdf")]
public async Task<IActionResult> DescargarReporteEjecutivoPdf(
    Guid sucursalId,
    DateTime fecha)
{
    var corte = await _mediator.Send(new GetCorteCajaQuery
    {
        SucursalId = sucursalId,
        Fecha = fecha
    });

    var reporte = await _mediator.Send(new GetReporteVentasQuery
    {
        FechaInicio = fecha.Date,
        FechaFin = fecha.Date.AddDays(1),
        SucursalId = sucursalId
    });

    var pdf = _pdfGenerator.GenerarReporteEjecutivoPdf(corte, reporte);

    return File(
        pdf,
        "application/pdf",
        $"ReporteEjecutivo_{fecha:yyyyMMdd}.pdf"
    );
}


}
