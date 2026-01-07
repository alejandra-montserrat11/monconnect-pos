using MonConnect.Application.Ventas.DTOs;
using MonConnect.Application.Exports.DTOs;

public interface IExcelExportService
{
    ExcelFileDto ExportReporteVentas(ReporteVentasDto reporte);
    ExcelFileDto ExportCorteCaja(CorteCajaDto corte);
     ExcelFileDto ExportProductosSinMovimiento(
        List<ProductoSinMovimientoDto> productos
    );
}
