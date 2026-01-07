
using MediatR;
using MonConnect.Application.Exports.DTOs;

public record ExportReporteVentasExcelQuery(
    DateTime FechaInicio,
    DateTime FechaFin,
    Guid? SucursalId
) : IRequest<ExcelFileDto>;

public record ExportCorteCajaExcelQuery(
    DateTime Fecha,
    Guid SucursalId
) : IRequest<ExcelFileDto>;

//QUERY DE EXPORTACION PARA PRODUCTOS SIN MOVMIENTO
public record ExportProductosSinMovimientoExcelQuery(
    int Dias,
    Guid? SucursalId
) : IRequest<ExcelFileDto>;
