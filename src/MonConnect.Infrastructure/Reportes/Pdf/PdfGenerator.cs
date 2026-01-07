
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
//using MonConnect.Application.Reportes.Services;

public class PdfGenerator : IPdfGenerator
{
    //codigo para generar el corte de caja en pdf
    public byte[] GenerarCorteCajaPdf(CorteCajaDto corte)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header().Text("CORTE DE CAJA")
                    .FontSize(20)
                    .Bold()
                    .AlignCenter();

                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text($"Fecha: {corte.Fecha:dd/MM/yyyy}");
                    col.Item().Text($"Sucursal: {corte.SucursalNombre}");
                    col.Item().Text($"Total ventas: {corte.TotalVentas}");
                    col.Item().Text($"Total vendido: ${corte.TotalVendido:N2}");

                    col.Item().LineHorizontal(1);

                    col.Item().Text(corte.Mensaje)
                        .Italic()
                        .FontSize(10);
                });

                page.Footer()
                    .AlignCenter()
                    .Text("Generado por MonConnect POS")
                    .FontSize(9);
            });
        }).GeneratePdf();
    }

    //Codigo para generar el pdf de reporte de ventas
    public byte[] GenerarReporteVentasPdf(ReporteVentasDto reporte)
{
    return Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(30);

            page.Header().Text("REPORTE DE VENTAS")
                .FontSize(20)
                .Bold()
                .AlignCenter();

            page.Content().Column(col =>
            {
                col.Spacing(10);

                col.Item().Text($"Periodo:");
                col.Item().Text($"{reporte.FechaInicio:dd/MM/yyyy} - {reporte.FechaFin:dd/MM/yyyy}");

                if (!string.IsNullOrEmpty(reporte.SucursalNombre))
                {
                    col.Item().Text($"Sucursal: {reporte.SucursalNombre}");
                }

                col.Item().LineHorizontal(1);

                foreach (var item in reporte.Ventas)
                {
                    col.Item().Text(
                        $"{item.Fecha:dd/MM/yyyy} | {item.Total:C}"
                    );
                }

                col.Item().LineHorizontal(1);

                col.Item().Text($"Total vendido: {reporte.TotalVentas:C}")
                    .Bold();
            });

            page.Footer()
                .AlignCenter()
                .Text("Generado por MonConnect POS")
                .FontSize(9);
        });
    }).GeneratePdf();
}

public byte[] GenerarReporteEjecutivoPdf(
    CorteCajaDto corte,
    ReporteVentasDto reporte)
{
    return Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(30);

            page.Header()
                .Text("REPORTE EJECUTIVO")
                .FontSize(22)
                .Bold()
                .AlignCenter();

            page.Content().Column(col =>
            {
                col.Spacing(20);

                // 🔹 SECCIÓN 1 — CORTE DE CAJA
                col.Item().Text("CORTE DE CAJA")
                    .FontSize(16)
                    .Bold();

                col.Item().Text($"Fecha: {corte.Fecha:dd/MM/yyyy}");
                col.Item().Text($"Sucursal: {corte.SucursalNombre}");
                col.Item().Text($"Total ventas: {corte.TotalVentas}");
                col.Item().Text($"Total vendido: ${corte.TotalVendido:N2}");

                col.Item().LineHorizontal(1);

                // 🔹 SECCIÓN 2 — REPORTE DE VENTAS
                col.Item().Text("DETALLE DE VENTAS")
    .FontSize(16)
    .Bold();

foreach (var venta in reporte.Ventas)
{
    col.Item().PaddingTop(10).Text(
        $"Venta: {venta.Fecha:dd/MM/yyyy HH:mm} {venta}  |  Total: {venta.Total:C}")
        .Bold();

    col.Item().Table(table =>
    {
        table.ColumnsDefinition(columns =>
        {
            columns.RelativeColumn(4); // Producto
            columns.RelativeColumn(2); // Cantidad
            columns.RelativeColumn(2); // Precio
            columns.RelativeColumn(2); // Subtotal
        });

        table.Header(header =>
        {
            header.Cell().Text("Producto").Bold();
            header.Cell().Text("Cantidad").Bold();
            header.Cell().Text("Precio").Bold();
            header.Cell().Text("Subtotal").Bold();
        });

        foreach (var detalle in venta.Detalles)
        {
            table.Cell().Text(detalle.ProductoNombre);
            table.Cell().Text(detalle.Cantidad.ToString());
            table.Cell().Text(detalle.PrecioUnitario.ToString("C"));
            table.Cell().Text(
                (detalle.Cantidad * detalle.PrecioUnitario).ToString("C"));
        }
    });

    col.Item().LineHorizontal(0.5f);
}


                col.Item().Text($"TOTAL PERIODO: {reporte.MontoTotal:C}")
                    .Bold();
            });

            page.Footer()
                .AlignCenter()
                .Text("Generado por MonConnect POS")
                .FontSize(9);
        });
    }).GeneratePdf();
}

}
