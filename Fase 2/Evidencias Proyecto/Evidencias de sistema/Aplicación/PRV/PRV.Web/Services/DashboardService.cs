
using PRV.Web.Models;
using PRV.Web.Repositories;

namespace PRV.Web.Services
{
    public class DashboardService
    {
        private readonly DashboardRepository _repository;

        public DashboardService(DashboardRepository repository)
        {
            _repository = repository;
        }

        public DashboardResumen ObtenerResumen(
            long idEmpresa, int anio, int? mes)
        {
            // Definir fechas según año y mes
            DateTime fechaDesde = new DateTime(
                anio, mes ?? 1, 1);

            DateTime fechaHasta = mes.HasValue
                ? fechaDesde.AddMonths(1).AddDays(-1)
                : fechaDesde.AddYears(1).AddDays(-1);

            // Consultar información
            var ventas = _repository.ListarVentas(
                idEmpresa, fechaDesde, fechaHasta);

            var confirmadas = ventas
                .Where(v => v.Estado == "Confirmada")
                .ToList();

            var anuladas = ventas
                .Where(v => v.Estado == "Anulada")
                .ToList();

            // Preparar resumen
            var resumen = new DashboardResumen
            {
                CantidadConfirmadas = confirmadas.Count,
                TotalConfirmadas = confirmadas.Sum(v => v.Total),

                CantidadAnuladas = anuladas.Count,
                TotalAnuladas = anuladas.Sum(v => v.Total),

                ProductosActivos =
                    _repository.ContarProductosActivos(idEmpresa),

                SolicitudesPendientes =
                    _repository.ContarSolicitudesPendientes(idEmpresa),

                Ventas = ventas
            };

            // Agrupar ventas confirmadas por mes o día
            resumen.Grafico = confirmadas
                .GroupBy(v => mes.HasValue
                    ? v.FechaVenta.Day
                    : v.FechaVenta.Month)
                .Select(g => new DashboardGrafico
                {
                    Periodo = g.Key,
                    Total = g.Sum(v => v.Total)
                })
                .OrderBy(g => g.Periodo)
                .ToList();

            return resumen;
        }
    }
}
