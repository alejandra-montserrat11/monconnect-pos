
using MediatR;
using MonConnect.Application.Exports.DTOs;
using MonConnect.Application.Ventas.Queries;

public class ExportReporteVentasExcelQueryHandler
    : IRequestHandler<ExportReporteVentasExcelQuery, ExcelFileDto>
{
    private readonly IMediator _mediator;
    private readonly IExcelExportService _excelService;

    public ExportReporteVentasExcelQueryHandler(
        IMediator mediator,
        IExcelExportService excelService)
    {
        _mediator = mediator;
        _excelService = excelService;
    }
public async Task<ExcelFileDto> Handle(
    ExportReporteVentasExcelQuery request,
    CancellationToken cancellationToken)
{
    // 1️⃣ Reutilizamos tu query existente (FORMA CORRECTA)
    var reporte = await _mediator.Send(
        new GetReporteVentasQuery
        {
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            SucursalId = request.SucursalId
        },
        cancellationToken
    );

    // 2️⃣ Generamos Excel
    return _excelService.ExportReporteVentas(reporte);
}

public class ExportCorteCajaExcelQueryHandler
    : IRequestHandler<ExportCorteCajaExcelQuery, ExcelFileDto>
{
    private readonly IMediator _mediator;
    private readonly IExcelExportService _excelService;

    public ExportCorteCajaExcelQueryHandler(
        IMediator mediator,
        IExcelExportService excelService)
    {
        _mediator = mediator;
        _excelService = excelService;
    }

    public async Task<ExcelFileDto> Handle(
    ExportCorteCajaExcelQuery request,
    CancellationToken cancellationToken)
{
    // 1️⃣ Reutilizamos query existente (FORMA CORRECTA)
    var corte = await _mediator.Send(
        new GetCorteCajaQuery
        {
            Fecha = request.Fecha,
            SucursalId = request.SucursalId
        },
        cancellationToken
    );

    // 2️⃣ Generamos Excel
    return _excelService.ExportCorteCaja(corte);
}
}

//GENERAR EXCEL PARA PRODUCTOS SIN MOVIMIENTO
public class ExportProductosSinMovimientoExcelQueryHandler
    : IRequestHandler<ExportProductosSinMovimientoExcelQuery, ExcelFileDto>
{
    private readonly IMediator _mediator;
    private readonly IExcelExportService _excelService;

    public ExportProductosSinMovimientoExcelQueryHandler(
        IMediator mediator,
        IExcelExportService excelService)
    {
        _mediator = mediator;
        _excelService = excelService;
    }


     public async Task<ExcelFileDto> Handle(
    ExportProductosSinMovimientoExcelQuery request,
    CancellationToken cancellationToken)
{
    // 1️⃣ Reutilizamos query existente (FORMA CORRECTA)
    var productos = await _mediator.Send(
        new GetProductosSinMovimientoQuery
        {
            Dias = request.Dias,
            SucursalId = request.SucursalId
        },
        cancellationToken
    );

    // 2️⃣ Generamos Excel
    return _excelService.ExportProductosSinMovimiento(productos);
}
}

}
