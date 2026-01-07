using ClosedXML.Excel;
using MonConnect.Application.Exports.DTOs;
using MonConnect.Application.Ventas.DTOs;

public class ExcelExportService : IExcelExportService
{
    public ExcelFileDto ExportReporteVentas(ReporteVentasDto reporte)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Reporte de Ventas");

        // ENCABEZADOS
        worksheet.Cell(1, 1).Value = "Fecha";
        worksheet.Cell(1, 2).Value = "Sucursal";
        worksheet.Cell(1, 3).Value = "Total Venta";

        var row = 2;

        foreach (var venta in reporte.Ventas)
        {
            worksheet.Cell(row, 1).Value = venta.Fecha;
            worksheet.Cell(row, 2).Value = venta.SucursalNombre;
            worksheet.Cell(row, 3).Value = venta.Total;
            row++;
        }

        // TOTALES
        worksheet.Cell(row + 1, 2).Value = "TOTAL";
        worksheet.Cell(row + 1, 3).Value = reporte.MontoTotal;

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return new ExcelFileDto
        {
            Content = stream.ToArray(),
            FileName = $"ReporteVentas_{DateTime.UtcNow:yyyyMMddHHmm}.xlsx"
        };
    }

//PARA CORTE DE CAJA
    public ExcelFileDto ExportCorteCaja(CorteCajaDto corte)
{
    using var workbook = new XLWorkbook();
    var sheet = workbook.Worksheets.Add("Corte de Caja");

    sheet.Cell(1, 1).Value = "Fecha";
    sheet.Cell(1, 2).Value = corte.Fecha.ToString("dd/MM/yyyy");

    sheet.Cell(2, 1).Value = "Sucursal";
    sheet.Cell(2, 2).Value = corte.SucursalNombre;

    sheet.Cell(4, 1).Value = "Total de Ventas";
    sheet.Cell(4, 2).Value = corte.TotalVentas;

    sheet.Cell(5, 1).Value = "Total Vendido";
    sheet.Cell(5, 2).Value = corte.TotalVendido;

    sheet.Cell(7, 1).Value = "Mensaje";
    sheet.Cell(7, 2).Value = corte.Mensaje;

    sheet.Columns().AdjustToContents();

    using var stream = new MemoryStream();
    workbook.SaveAs(stream);

    return new ExcelFileDto
    {
        Content = stream.ToArray(),
        FileName = $"CorteCaja_{corte.Fecha:yyyyMMdd}_{corte.SucursalNombre}.xlsx"
    };
}


//PARA PRODUCTOS SIN MOVIMEINTO
public ExcelFileDto ExportProductosSinMovimiento(
    List<ProductoSinMovimientoDto> productos)
{
    using var workbook = new XLWorkbook();
    var sheet = workbook.Worksheets.Add("Productos Sin Movimiento");

    // Encabezados
    sheet.Cell(1, 1).Value = "Producto";
    sheet.Cell(1, 2).Value = "Sucursal";
    sheet.Cell(1, 3).Value = "Última Venta";
    sheet.Cell(1, 4).Value = "Días Sin Movimiento";

    var row = 2;

    foreach (var p in productos)
    {
        sheet.Cell(row, 1).Value = p.ProductoNombre;
        sheet.Cell(row, 2).Value = p.SucursalNombre;
        sheet.Cell(row, 3).Value = p.UltimaVenta?.ToString("dd/MM/yyyy") ?? "Nunca";
        sheet.Cell(row, 4).Value = p.DiasSinMovimiento;
        row++;
    }

    sheet.Columns().AdjustToContents();

    using var stream = new MemoryStream();
    workbook.SaveAs(stream);

    return new ExcelFileDto
    {
        Content = stream.ToArray(),
        FileName = $"ProductosSinMovimiento_{DateTime.Now:yyyyMMdd}.xlsx"
    };
}
}
