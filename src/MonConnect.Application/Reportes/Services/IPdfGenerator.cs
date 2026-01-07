public interface IPdfGenerator
{
    byte[] GenerarCorteCajaPdf(CorteCajaDto corte);
    byte[] GenerarReporteVentasPdf(ReporteVentasDto reporte);
    byte[] GenerarReporteEjecutivoPdf(CorteCajaDto corte, ReporteVentasDto reporte);
}
