
namespace PRV.Web.Models
{
    public class DashboardResumen
    {
        public int CantidadConfirmadas { get; set; }
        public decimal TotalConfirmadas { get; set; }

        public int CantidadAnuladas { get; set; }
        public decimal TotalAnuladas { get; set; }

        public int ProductosActivos { get; set; }
        public int SolicitudesPendientes { get; set; }

        public List<DashboardGrafico> Grafico { get; set; } = new();
        public List<DashboardVenta> Ventas { get; set; } = new();
    }

    public class DashboardGrafico
    {
        public int Periodo { get; set; }
        public decimal Total { get; set; }
    }
}
